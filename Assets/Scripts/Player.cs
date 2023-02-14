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

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

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
    }

    private void VerticalMovement()
    {
        if (_isCanJump && SimpleInput.GetAxis("Vertical") > 0)
        {
            _isCanJump = false;
            _rigidbody.AddForce(jumpImpulse * Vector2.up, ForceMode2D.Impulse);
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
    }
}