using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public enum State
    {
        Calm,
        Haunt,
        Special
    }

    public abstract class EnemyControllerBase : MonoBehaviour
    {
        protected EnemyAnimationController anim;
        protected Rigidbody2D rb;
        private DamageReg incomingDamage;
        protected AttackZoneTrigger attackTarget;
        protected AggroTrigger playerFind;
        protected EnemyHealthBar enemyHealth;

        protected Vector3 target;
        protected Vector3 pointOne;
        protected Vector3 pointTwo;

        [Header("References")]
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private LayerMask playerLayers;
        [SerializeField] private Transform p1;
        [SerializeField] private Transform p2;

        [Header("Parameters")]
        [SerializeField] protected float _speed = 1.5f;
        [SerializeField] protected float _chaseSpeed = 3f;
        [SerializeField] protected float _attackDamage = 10f;
        [SerializeField] private float _attackRange = 0.5f;
        [SerializeField] protected float _attackDelay = 1f;
        [SerializeField] protected float _damagedForce = 5f;
        [SerializeField] protected float _dieDelay = 1f;

        protected Vector2 takeDamageOffset = new Vector2(-3, 1).normalized;

        protected float _defaultMaxHealth = 100f;
        [SerializeField] protected float _currentHealth;

        private Transform currentLevel;
        private GameObject findPlayer;
        protected State currentState;
        protected bool isPlayerInRange = false;
        private bool trackPlayer = false;
        protected bool isAttacking = false;
        protected bool isActive = true;

        private void Awake()
        {
            enemyHealth = GetComponentInChildren<EnemyHealthBar>();
            incomingDamage = GetComponent<DamageReg>();
            rb = GetComponent<Rigidbody2D>();
            attackTarget = GetComponentInChildren<AttackZoneTrigger>();
            playerFind = GetComponentInChildren<AggroTrigger>();
            anim = GetComponent<EnemyAnimationController>();
            currentLevel = FindObjectOfType<Level>().transform;
        }

        private void Start()
        {
            incomingDamage.TakeDamage += TakeDamage;
            attackTarget.PlayerEnterRange += FindAttackTarget;
            attackTarget.PlayerCancelRange += LostAttackTarget;
            playerFind.FindTarget += FindTarget;
            playerFind.LostTarget += LostTarget;
            
            pointOne = p1.transform.position;
            pointTwo = p2.transform.position;

            _currentHealth = _defaultMaxHealth;
            currentState = State.Calm;
            DefaultActions();
            SetTarget();
        }

        private void OnDestroy()
        {
            incomingDamage.TakeDamage -= TakeDamage;
            attackTarget.PlayerEnterRange -= FindAttackTarget;
            attackTarget.PlayerCancelRange -= LostAttackTarget;
            playerFind.FindTarget -= FindTarget;
            playerFind.LostTarget -= LostTarget;
        }

        private void FindAttackTarget()
        {
            isPlayerInRange = true;
        }

        private void LostAttackTarget()
        {
            isPlayerInRange = false;
        }

        private void SetTarget()
        {
            target = Random.Range(0, 2) == 0 ? pointOne : pointTwo;
        }

        protected abstract void DefaultActions();

        protected abstract void AttackActions();

        private void FixedUpdate()
        {
            if (!isActive)
                return;

            if (currentState == State.Calm)
            {
                CalmActions();
            }

            if (currentState == State.Haunt)
            {
                ChasePlayer();
            }

            if (currentState == State.Special)
            {
                SpecialActions();
            }
        }

        private void Update()
        {
            if (trackPlayer)
            {
                target = findPlayer.transform.position;
            }
            
            if (isPlayerInRange)
            {
                DefaultAttack();
            }

            p1.position = pointOne;
            p2.position = pointTwo;
        }

        protected virtual void CalmActions()
        {
            if (isAttacking)
                return;

            MoveToTarget(_speed);
            var targetX = Mathf.RoundToInt(target.x);
            var ourX = Mathf.RoundToInt(transform.position.x);

            if (ourX == targetX)
            {
                target = target == pointOne ? pointTwo : pointOne;
            }

            anim.Move(true);
        }

        protected virtual void ChasePlayer()
        {
            if (isAttacking)
                return;

            MoveToTarget(_chaseSpeed);
            anim.Move(true);
        }

        protected abstract void SpecialActions();

        protected void MoveToTarget(float speed)
        {
            var direction = (target - transform.position).normalized;
            var velocity = rb.velocity;
            velocity.x = direction.x * speed;
            rb.velocity = velocity;
            UpdateRotation((int)Mathf.Sign(direction.x));
        }

        private void UpdateRotation(int sign)
        {
            var localScale = transform.localScale;
            if ((int)Mathf.Sign(localScale.x) != sign)
            {
                localScale.x *= -1;
            }

            transform.localScale = localScale;
        }

        private void FindTarget(GameObject player)
        {
            findPlayer = player;
            trackPlayer = true;
            AttackActions();
        }

        private void LostTarget()
        {
            trackPlayer = false;
            currentState = State.Calm;
            SetTarget();
            anim.Special(false);
        }

        protected virtual void DefaultAttack()
        {
            if (isAttacking)
                return;
            
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(
                attackPoint.position, _attackRange, playerLayers);

            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<DamageReg>().TakingDamage(_attackDamage);
            }

            isAttacking = true;
            ResetAttack();

            anim.Attack();
        }

        protected async void ResetAttack()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_attackDelay));
            isAttacking = false;
        }

        protected virtual void TakeDamage(float damage)
        {
            anim.GetHurt();
            isAttacking = true;
            ResetAttack();
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(_damagedForce * (transform.localScale * takeDamageOffset), ForceMode2D.Impulse);
            
            _currentHealth -= damage;
            enemyHealth.SetCurrentHealth(_currentHealth);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            var coin = Instantiate(coinPrefab, transform);
            coin.transform.SetParent(currentLevel);
            gameObject.SetActive(false);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(attackPoint.position, _attackRange);
            Gizmos.DrawWireSphere(p1.position, 0.3f);
            Gizmos.DrawWireSphere(p2.position, 0.3f);
        }
    }
}