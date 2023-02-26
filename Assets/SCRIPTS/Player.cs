using System;
using Cinemachine;
using DefaultNamespace.TRIGGERS;
using SimpleInputNamespace;
using TRIGGERSand_CO;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float horisontalSpeed = 5f;
        [SerializeField] private float jumpImpulse = 5f;

        // компонент rigibody, ибо плеер физический и взаимодейсствует с физикой мира
        private Rigidbody2D _rigidbody;
        private bool _isActive = true;
        [SerializeField] private bool _isCanJump; // пока не приземлится шоб снова не начинал прыгать
        private bool _isDirty = false; 

        private float _maxVelocityMagnitude;
        // если двигается вверх чтобы не мог двигаться по горизонтали


        private PlayerAnimationController _animatorcontroller;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>(); // находим компонент
            _animatorcontroller = GetComponent<PlayerAnimationController>();
        }


        private void Start()
        {
            _maxVelocityMagnitude = Mathf.Sqrt(Mathf.Pow(jumpImpulse, 2f) + Mathf.Pow(horisontalSpeed, 2));
            // Mathf - это класс по математику
            // Pow(Single, Single) - Возвращает указанное число, возведенное в указанную степень.
            // квадрат джампа плюс квадрат хоризонтала и корень Sqrt
            // это теорема пифагора - чем быстрее вверх, тем ммедленне по горизонтали ://
            // короче оно замедляет

            //------------[◉"] Cinemachine [◉"]------------------------------
            var camera = FindObjectOfType<CinemachineVirtualCamera>();
            camera.m_Follow = transform;
            camera.m_LookAt = transform; // за зайцем
        }


        // все движ в фикседапдейте (в одном кадре) ибо физика
        private void FixedUpdate()
        {
            // проверить, если мы не активны - то и не делаем дальше функцию
            if (!_isActive)
                return;

            Movement(); // сначала двигаемся
            UpdateSide(); // в зависимости от куда попали - разворачиваем персонажа
        }



        private void Movement()
        {
            HorizontalMovement();
            VerticalMovement();
            ClampVelocity();
        }

        //----------------------- не пытайся это понять----------------------------
        private void HorizontalMovement()
        {
            var axis = SimpleInput.GetAxis("Horizontal"); // с плагина считываем, буква Z!
            var velocity = _rigidbody.velocity;

            velocity.x = horisontalSpeed * axis; // не нажали - ноль, влево -1, вправо на 1
            _rigidbody.velocity = velocity;

            _animatorcontroller.SetSpeed(velocity.x == 0 ? 0 : (int)Mathf.Sign(velocity.x));
            // Sign - Return value is 1 when f is positive or zero, -1 when f is negative. 
            // если равен рулю - включаем анимацию бега, передаем ноль
            // : если не ранен нулю - передаем туда единицу Mathf 
            // можно было передавать bool, или velocity.X или magnitude - для разных скоростей бега со своими анимашками


        }

        private void VerticalMovement() // разово прыжок
        {
            // если убрать /*_isCanJump &&*/ то заяц иногда внезапно застревает, не только в воде
            if (_isCanJump && SimpleInput.GetAxis("Vertical") > 0) // можем ли прыгать и нажали кнопку
            {
                // Тайное послание от бывшей училки английского:
                // грамматически может быть только "_canJump", или "_ableToJump" :))  а isCan нееее ))
                _isCanJump = false; // второй раз не прыгаем
                _rigidbody.AddForce(jumpImpulse * Vector2.up, ForceMode2D.Impulse); // пинаем себя вверх импульсом !!!

                _animatorcontroller.SetJump();
            }

        }

        private void ClampVelocity()
        {
            var velocity = _rigidbody.velocity.magnitude; // берем текущую длину вектора
            velocity = Mathf.Clamp(velocity, 0, _maxVelocityMagnitude);
            // запираем в диапазоне - от нуля до макс например 15

            _rigidbody.velocity = _rigidbody.velocity.normalized * velocity; // присваиваем обратно
            // направление нормализуем к единице чтобы получить только направление :/
            // 1 умножаем на то что посчиталось - оно и получится :////
            // ограничиваем скорость в том направлении, в котором мы двигались
            // The velocity vector of the rigidbody. It represents the rate of change of Rigidbody position.
            // A velocity in Unity is units per second.
            // Unity velocity also has the speed in X, Y, and Z defining the direction.
        }


        private void UpdateSide()
        {
            // а двигались ли мы в этом кадре
            if (!(Mathf.Abs(_rigidbody.velocity.x) > 0))
                // значение приведенное к МОДУЛЮ - всегда положительное
                return;

            // если двигаемся, то считаем дальше, в какую сторону

            // если флипаем, то лучше путём умножения на -1 !!!
            // умножить scale по Z на -1 если надо развернутьв 3D
            // x положительный если вправо, отрицательный если влево
            // Sign - Return value is 1 when f is positive or zero, -1 when f is negative.
            var sign = Mathf.Sign(_rigidbody.velocity.x);
            var scale = Mathf.Sign(transform.localScale.x);
            // берет значение локал скейл и получаем от него 1 или -1

            // если не совпадает - разворачиваем
            if (scale != sign)
            {
                var localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale; // присваиваем обратно
            }
        }


        //--------------------------- занятие 30 -----------------------------------------------------
        //------ сталкивается с платформой и может прыгать, или помирает если сталкивается с опасным
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!_isActive)
                return; // неактивны - вон из функции
            _isCanJump = true;
            
            if (col.gameObject.GetComponentInChildren<Platform>())
            {
                // если с платформой - то можно прыгать
                _isCanJump = true;
            }

            if (col.gameObject.GetComponent<DangerousObject>())
            {
                Stealing();
            }
            
            
        }

        //------ для сбора монеток -----------------
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!_isActive)
                return;

            //------ далее пять похожих проверок - как приавильно написать case?
            if (col.GetComponent<Coin>())
            {
                col.gameObject.SetActive(false);
                GameManager.Instance.OnCoinCollect();
            }


            if (col.GetComponent<Carrot>())
            {
                col.gameObject.SetActive(false);
                GameManager.Instance.OnCarrotCollect();
            }
            
            if (col.GetComponent<Berry>())
            {
                col.gameObject.SetActive(false);
                GameManager.Instance.OnBerryCollect();
            }

            if (col.GetComponent<Gem>())
            {
                col.gameObject.SetActive(false);
                GameManager.Instance.OnGemCollect();
            }

            if (col.GetComponent<GoldenPoo>())
            {
                col.gameObject.SetActive(false);
                GameManager.Instance.OnGoldenPooCollect();
            }

            //--------- эта касается только внешнего вида зайца, оставлю в рамках данного файла
            if (col.GetComponent<BrownPoo>())
            {
                col.gameObject.SetActive(false);
                GetDirty();
            }

            if (col.GetComponent<Water>())
            {
                GetClean();
            }
            
            //--------- враг ---------------------------
            if (col.GetComponent<DangerousObject>())
            {
                Stealing();
            }
            
            //--------- столкновение с флажком / домиком 
            if (col.GetComponent<Finish>())
            {
                if (!_isDirty)
                {GameManager.Instance.OnFinish();}
                else
                Debug.Log(" Грязному домой нельзя, помойся!");    
            }
        }


        //------------ после столкновения с какахой пачакается, водой - моется ---------
        private void GetDirty()
        {
            if (_isDirty == false)
            {
                _isDirty = true;
                _animatorcontroller.DirtyAnims();
                Debug.Log(" заяц грязный");
                // НАРИСОВАТЬ И ЗАМЕНИТЬ
            }
        }

        private void GetClean()
        {
            if (_isDirty == true)
            {
                _animatorcontroller.CleanAnims();
                Debug.Log(" заяц помылся");
                _isDirty = false;
            }
        }

        
        
        private void Stealing()
        {
            // _animatorcontroller.SetSpeed(0);
            // _isActive = false;
            // _rigidbody.simulated = false;
            GameManager.Instance.OnEnemySteal();
        }

    }
}