using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private DamageReg incomingDamage;
    private PlayerAnimationController animController;
    private new CapsuleCollider2D collider;
    
    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    [Header("Parameters")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float damagedForce = 2f;
    [SerializeField] private float dieDelay = 3f;
    [SerializeField] private float currentHealth;

    private float maxVelocityMagnitude;
    
    private Vector2 baseSize;
    private Vector2 baseOffset;
    private Vector2 crouchSize;
    private Vector2 crouchOffset;

    private Vector2 takeDamageOffset = new Vector2(-3, 1).normalized;

    private bool isActive;
    private bool isGrounded;
    private bool isCrouching;
    private bool isAttacking = false;

    public bool IsGrounded => isGrounded;
    public bool IsCrouching => isCrouching;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        incomingDamage = GetComponent<DamageReg>();
        animController = GetComponent<PlayerAnimationController>();
        collider = GetComponent<CapsuleCollider2D>();
        
        var camera = FindObjectOfType<CinemachineVirtualCamera>();
        camera.m_Follow = transform;
        camera.m_LookAt = transform;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        GameManager.Instance.RefreshHealthBar(currentHealth);
        incomingDamage.TakeDamage += TakeDamage;
        
        maxVelocityMagnitude = Mathf.Sqrt(Mathf.Pow(jumpForce, 2) + Mathf.Pow(speed, 2));
        
        baseSize = collider.size;
        baseOffset = collider.offset;
        crouchSize = baseSize;
        crouchOffset = baseOffset;
        crouchSize.y *= 0.5f;
        crouchOffset.y -= 0.47f;

        isActive = true;
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        GameManager.Instance.RefreshHealthBar(currentHealth);
        rb.bodyType = RigidbodyType2D.Dynamic;
        isActive = true;
    }

    private void OnDestroy()
    {
        incomingDamage.TakeDamage -= TakeDamage;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        
        DoJump();
        Attack();
    }

    private void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }
        
        StateCheck();
        UpdateRotation();
        Move();
    }

    private void StateCheck()
    {
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position,
            1 << LayerMask.NameToLayer("Ground"));

        if (isGrounded)
        {
            isCrouching = SimpleInput.GetAxis("Vertical") < 0;
        }
        
        animController.SetFly(!isGrounded);
        animController.SetCrouch(isCrouching);
    }

    private void UpdateRotation()
    {
        if (!isAttacking)
        {
            var axis = SimpleInput.GetAxis("Horizontal");
            if (axis > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (axis < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void Move()
    {
        if (!isAttacking)
        {
            HorizontalMove();
            Crouching();
            ClampVelocity();
        }
    }

    private void HorizontalMove()
    {
        if (!isCrouching)
        {
            var axis = SimpleInput.GetAxis("Horizontal");
            var velocity = rb.velocity;
            velocity.x = speed * axis;
            rb.velocity = velocity;
            animController.SetRun(Mathf.Abs(Mathf.RoundToInt(velocity.x)));
        }
    }

    private void DoJump()
    {
        if ((SimpleInput.GetKeyDown(KeyCode.W) && isGrounded && !isAttacking) || 
            (SimpleInput.GetButtonDown("Jump") && isGrounded && !isAttacking))
        {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            animController.SetJump();
        }
    }

    private void Crouching()
    {
        if (isCrouching)
        {
            collider.size = crouchSize;
            collider.offset = crouchOffset;
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            collider.size = baseSize;
            collider.offset = baseOffset;
        }
    }

    private void ClampVelocity()
    {
        var velocity = rb.velocity.magnitude;
        velocity = Mathf.Clamp(velocity, 0, maxVelocityMagnitude);
        rb.velocity = rb.velocity.normalized * velocity;
    }

    private void Attack()
    {
        if (isAttacking || isCrouching)
            return;

        if (SimpleInput.GetKeyDown(KeyCode.J) || SimpleInput.GetButtonDown("Attack"))
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(
                attackPoint.position, attackRange, enemyLayers);

            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<DamageReg>().TakingDamage(attackDamage);
            }

            isAttacking = true;
            ResetAttack();

            if (isGrounded)
            {
                rb.velocity = new Vector2(0, 0);
                animController.SetAttack();
            }
            else
            {
                animController.SetFlyAttack();
            }
        }
    }

    private async void ResetAttack()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(attackDelay));
        isAttacking = false;
    }
    
    private void TakeDamage(float damage)
    {
        if (!isActive)
            return;
        
        currentHealth -= damage;
        GameManager.Instance.RefreshHealthBar(currentHealth);
        isAttacking = true;
        ResetAttack();
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(damagedForce * (transform.localScale * takeDamageOffset), ForceMode2D.Impulse);
        animController.SetTakeDamage();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<CoinComponent>())
        {
            col.gameObject.SetActive(false);
            GameManager.Instance.OnCoinCollect();
        }
        
        if (col.GetComponent<FinishComponent>())
        {
            GameManager.Instance.OnFinish();
        }
    }
    
    private async void Die()
    {
        isActive = false;
        rb.bodyType = RigidbodyType2D.Static;
        await UniTask.Delay(TimeSpan.FromSeconds(dieDelay));
        GameManager.Instance.OnPlayerDeath();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}