using System;
using Cinemachine;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float jumpImpulse = 5f;

    private Rigidbody2D _rigidbody;
    private bool _isActive = true;
    private bool _isCanJump;

    private float _maxVelocityMagnitude;

    private PlayerAnimationController _animationController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animationController = GetComponent<PlayerAnimationController>();
    }

    private void Start()
    {
        _maxVelocityMagnitude = Mathf.Sqrt(Mathf.Pow(jumpImpulse, 2f) + Mathf.Pow(horizontalSpeed, 2));
        var camera = FindObjectOfType<CinemachineVirtualCamera>();
        camera.m_Follow = transform;
        camera.m_LookAt = transform;
    }

    private void FixedUpdate()
    {
        if (!_isActive)
            return;

        Movement();
        UpdateSide();
    }

    private void Movement()
    {
        HorizontalMovement();
        VerticalMovement();
        ClampVelocity();
    }

    private void HorizontalMovement()
    {
        var axis = SimpleInput.GetAxis("Horizontal");
        var velocity = _rigidbody.velocity;
        velocity.x = horizontalSpeed * axis;
        _rigidbody.velocity = velocity;
        _animationController.SetSpeed(velocity.x == 0 ? 0 : (int) Mathf.Sign(velocity.x));
    }

    private void VerticalMovement()
    {
        if (_isCanJump && SimpleInput.GetAxis("Vertical") > 0)
        {
            _isCanJump = false;
            _rigidbody.AddForce(jumpImpulse * Vector2.up, ForceMode2D.Impulse);
            _animationController.SetJump();
        }
    }

    private void ClampVelocity()
    {
        var velocity = _rigidbody.velocity.magnitude;
        velocity = Mathf.Clamp(velocity, 0, _maxVelocityMagnitude);
        _rigidbody.velocity = _rigidbody.velocity.normalized * velocity;
    }

    private void UpdateSide()
    {
        if (!(Mathf.Abs(_rigidbody.velocity.x) > 0))
            return;
        var sign = Mathf.Sign(_rigidbody.velocity.x);
        var scale = Mathf.Sign(transform.localScale.x);
        if (scale != sign)
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!_isActive)
            return;
        if (col.gameObject.GetComponentInChildren<Platform>())
        {
            _isCanJump = true;
        }

        if (col.gameObject.GetComponent<DangerousObject>())
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!_isActive)
            return;

        if (col.GetComponent<Coin>())
        {
            col.gameObject.SetActive(false);
            GameManager.Instance.OnCoinCollect();
        }

        if (col.GetComponent<Finish>())
        {
            GameManager.Instance.OnFinish();
        }
    }
    
    private void Death()
    {
        _animationController.SetSpeed(0);
        _isActive = false;
        _rigidbody.simulated = false;
        GameManager.Instance.OnPlayerDeath();
    }

}