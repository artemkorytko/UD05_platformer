using System;
using Cinemachine;
using DefaultNamespace.ConfigsSO;
using DefaultNamespace.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PlayerAnimatorController), typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerConfig config;
        [SerializeField] private LayerMask layerMask;

        private Rigidbody2D _rigidbody;
        private PlayerAnimatorController _animator;
        
        private bool _isActive = true;
        private bool _isCanJump;
        private bool _isCanMove;

        private float _maxVelocityMagnitude;
        private int _currentHealth;

        private float _distanceRayToDown;
        private float _distanceRayToForward;


        private void Awake()
        {
            _animator = GetComponent<PlayerAnimatorController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            
            _distanceRayToDown = transform.localScale.y;
            _distanceRayToForward = transform.localScale.x * 0.8f;
        }

        private void Start()
        {
            _maxVelocityMagnitude = Mathf.Sqrt(Mathf.Pow(config.JumpImpulse, 2f) + Mathf.Pow(config.HorizontalSpeed, 2));
            
            var camera = FindObjectOfType<CinemachineVirtualCamera>();
            camera.m_Follow = transform;
            camera.m_LookAt = transform;
            
            GameManager.Instance.OnRestartLevel += OnRestart;
            
            _currentHealth = config.Health;
            GamePanel.Instance.AddHeard(_currentHealth);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if(!_isActive)
                return;
            
            if(other.gameObject.TryGetComponent(out RipEnemy enemy))
                enemy.Rip();
            
            
            if (other.gameObject.GetComponent<DangerousObject>() || other.gameObject.GetComponent<Spike>()) 
                TakeDamage();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Coin>())
            {
                other.gameObject.SetActive(false);
                GameManager.Instance.OnCoinCollect();
            }

            if (other.GetComponent<Finish>())
                GameManager.Instance.OnFinish();
        }

        private void FixedUpdate()
        {
            if(!_isActive)
                return;
            
            AllowToMovement();
            Movement();
            UpdateSide();
        }

        private void AllowToMovement()
        {
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -transform.up, _distanceRayToDown, layerMask);
            Debug.DrawRay(transform.position, -transform.up, Color.red);
            if (hitDown.collider != null)
                _isCanJump = true;
            
            RaycastHit2D hitForward = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0), _distanceRayToForward, layerMask);
            Debug.DrawRay(transform.position, new Vector2(transform.localScale.x, 0), Color.red); // т.к мы изменяем Х то луч тож будет менять направление.норм.. 
            if (hitForward.collider != null)
                _isCanMove = false;
            else
                _isCanMove = true;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnRestartLevel -= OnRestart;
        }

        private void UpdateSide()
        {
            if(!(Mathf.Abs(_rigidbody.velocity.x) > 0))
                return;

            var sing = Mathf.Sign(_rigidbody.velocity.x);
            var scale = Mathf.Sign(transform.localScale.x);

            if (scale != sing)
            {
                var localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }
        
        private void Movement()
        {
            HorizontalMovement();
            VerticalMovement();
            ClampVelocity();
        }

        private void ClampVelocity()
        {
            var velocity = _rigidbody.velocity.magnitude;
            velocity = Mathf.Clamp(velocity, 0, _maxVelocityMagnitude);
            _rigidbody.velocity = _rigidbody.velocity.normalized * velocity;
        }

        private void VerticalMovement()
        {
            if (_isCanJump && SimpleInput.GetAxis("Vertical") > 0)
            {
                _isCanJump = false;
                _animator.SetJump();
                _rigidbody.AddForce(config.JumpImpulse * Vector2.up, ForceMode2D.Impulse);
            }
        }

        private void HorizontalMovement()
        {
            var axis =  SimpleInput.GetAxis("Horizontal");
            var velocity = _rigidbody.velocity;
            velocity.x = config.HorizontalSpeed * axis;
            
            if (_isCanMove)
                _rigidbody.velocity = velocity;
            
            _animator.SetSpeed(velocity.x == 0 ? 0 : (int)Mathf.Sign(velocity.x));
            
        }
        
        private void TakeDamage()
        {
            GamePanel.Instance.RemoveHeard();
            
            if (--_currentHealth <= 0)
            {
                _isActive = false;
                gameObject.SetActive(false);
                _animator.SetSpeed(0);
                
                GameManager.Instance.OnPlayerDeath();
            }
        }
        private void OnRestart()
        {
            _currentHealth = config.Health;
            GamePanel.Instance.AddHeard(_currentHealth);
            
            _isActive = true;
            gameObject.SetActive(true);
        }
        
    }
}