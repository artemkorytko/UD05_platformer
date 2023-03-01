using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TRIGGERSand_CO;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform pointOne;
    [SerializeField] private Transform pointTwo;
    [SerializeField] private float speed;

    //-------- что может украсть -----------
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject carrot;
    [SerializeField] private GameObject berry;
    [SerializeField] private GameObject goldenPoo;
    [SerializeField] private GameObject gem;

        // место, куда будем инстантиэйтить украденное
    [SerializeField] private Transform _StealPoint;
    private GameData _EnemyGameData;
    private Dictionary <int, GameObject> _stealDick;
    
    // ссылка на текущую точку, к которой мы идём
    private Transform _target;

    // считает сколько стырил
    private int stealCount = 0;
    [SerializeField] private const float _stealOffset = 1;
    
    // ссылка на аниматор
    private EnemyAnimationController _animationController;

    private void Awake()
    {
        _animationController = GetComponent<EnemyAnimationController>();

        GameManager.Instance.OnFailEvent += StealOneThing; // плеер шлет в GM, тот сюда
        
    }

    private void Start()
    {
        // создаем себе цель куда идем, рандомно из двух добавленных 
        _target = Random.Range(0, 2) == 0 ? pointOne : pointTwo;
        
        _StealPoint = GetComponentInChildren<StealPoint>().transform;
        
        _stealDick = new Dictionary<int, GameObject>();
    }

    // ходит БЕЗ rigidboby поэтому в апдейте
    private void FixedUpdate()
    {
        // направление куда мы идем: от позиции цели отнять нашу позицию, получаем вектор куда идти
        // When normalized, a vector keeps the same direction but its length is 1.0.
        var direction = (_target.position - transform.position).normalized;
        
        // получаем дистанцию до цели, сколько осталось в этом кадре до цели
        // var distanceToTargetMag = (_target.position - transform.position).magnitude;
        // или так: (боги там внутри корень квадратов...)
        var distanceToTarget = Vector3.Distance(_target.position, transform.position);
        
        // считаем путь, сколько мы можем пройти за один кадр? ВО КАК!
        var oneFrameDist = speed * Time.deltaTime;

        // если цель ближе чем мы за кадр можем пройти - то последний шаг будет не целый, а остаточный кусок
        if (oneFrameDist > distanceToTarget)
        {
            // в сколько проййти за кадр записываем че осталось
            oneFrameDist = distanceToTarget; 
            
            // значит что дошли до точки и надо выбрать другую точку
            //обноаляем таргут
            _target = _target == pointOne ? pointTwo : pointOne;
            // если текущая цель равнна поинт1 - значит идем к поинту2, в обратном случае к поинт1
            //         если == п1 ?       - идем п2  : иначе п1 (значит что ==п2)
        }

        // и наконец идем!!!!! УМНОЖИТЬ О_о 1 / -1 (тут или путь за кадр, или оставшийся кусок
        transform.Translate(direction * oneFrameDist);

        // если изменилась цель, то в следующем кадре изменится дирекшен, сделаем шаг и развернемся
        // куда повернуться direction - приводим к инту и шлем его в функцию
        UpdateSide((int) Mathf.Sign(direction.x));
        
        _animationController.SetMove((int) Mathf.Sign(direction.x)); // идет в контроллер анимации врага
    }

    // получает инт, называет sign ---------- флипает врага, или не флипает
    private void UpdateSide(int sign)
    {
        // берем с врага какой у него ща скейл
        var localScale = transform.localScale;
        
        // из скейла по X делаем -1 или 1
        // если полученный инт из текущего транслейта не равен переданному инту вычесленного направления куда идем¯\_(ツ)_/¯ 
        if ((int) Mathf.Sign(localScale.x) != sign)
        {
            localScale.x *= -1; // тогда флипаем домножив на -1
        }

        transform.localScale = localScale; // присваиваем его нашему трансорму
    }

    //--------------------------------------------------------------------------------------------------
    private void StealOneThing()
    {
        

        _EnemyGameData = GameManager.Instance._gameData;

        //---- собирает словарь вещей, которые заяц уже насобирал и рандомно тырит одну
        int ii = 0;
        
        if (_EnemyGameData.Coins >= 1)
        {
            _stealDick.Add(ii, coin);
            ii++;
            GameManager.Instance._gameData.Coins--;
            GameManager.Instance.OnCoinCollect(true); // проверка на стыренность
        }

        if (_EnemyGameData.Berries >= 1)
        {
            _stealDick.Add(ii, berry);
            ii++;
            GameManager.Instance._gameData.Berries--;
            GameManager.Instance.OnBerryCollect(true);
        }

        if (_EnemyGameData.GoldenPoo >= 1)
        {
            _stealDick.Add(ii, goldenPoo);
            ii++;
            GameManager.Instance._gameData.GoldenPoo--;
            GameManager.Instance.OnGoldenPooCollect(true);
        }

        if (_EnemyGameData.Carrots >= 1)
        {
            _stealDick.Add(ii, carrot);
            ii++;
            GameManager.Instance._gameData.Carrots--;
            GameManager.Instance.OnCarrotCollect(true);
        }

        if (_EnemyGameData.Gems >= 1)
        {
            _stealDick.Add(ii, gem);
            GameManager.Instance._gameData.Gems--;
            GameManager.Instance.OnGemCollect(true);
        }
        //---------------------------------------
        
        

        
        //------- рандомно тырит вещь пока не заберет 6 штук
        if (_stealDick.Count != 0 && stealCount < 6) 
        {
            int rndsteal = Random.Range(0, _stealDick.Count);
            GameObject whatToSteal = Instantiate(_stealDick[rndsteal], _StealPoint.transform);
            
            float heightToPutThis = _stealOffset * stealCount;
            whatToSteal.transform.Translate(new Vector3(0,heightToPutThis, 0));

            whatToSteal.GetComponentInChildren<Collider2D>().enabled = false;
            // whatToSteal.
            
            stealCount++;
        }
        
        _stealDick.Clear();
    } // end of steal one thing 


    private void OnDestroy()
    {
        GameManager.Instance.OnFailEvent -= StealOneThing;
    }
}