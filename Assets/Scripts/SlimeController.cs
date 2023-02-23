using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public class SlimeController : EnemyControllerBase
    {
        [SerializeField] private float _maxHealth = 50f;
        [SerializeField] private float _specialHealthLimit = 40;
        [SerializeField] private float _specialSpeed = 4f;

        protected override void DefaultActions()
        {
            _currentHealth = _maxHealth;
            enemyHealth.SetCurrentHealth(_currentHealth);
        }
        
        protected override void AttackActions()
        {
            if (_currentHealth > Mathf.Round(_maxHealth * _specialHealthLimit * 0.01f))
            {
                currentState = State.Haunt;
            }
            else if (_currentHealth <= Mathf.Round(_maxHealth * _specialHealthLimit * 0.01f))
            {
                currentState = State.Special;
                anim.Move(false);
                anim.SpecialCharge(true);
            }
        }

        protected override void SpecialActions()
        {
            if (isAttacking)
                return;
                
            MoveToTarget(_specialSpeed);
        }

        protected override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            
            if (_currentHealth <= Mathf.Round(_maxHealth * _specialHealthLimit * 0.01f))
            {
                currentState = State.Special;
                anim.Move(false);
                anim.SpecialCharge(true);
            }
        }

        protected override async void Die()
        {
            isActive = false;
            anim.Special(false);
            anim.Die(true);
            rb.bodyType = RigidbodyType2D.Static;
            attackTarget.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(_dieDelay));
            base.Die();
        }
    }
}