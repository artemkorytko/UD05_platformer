using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private Animator anim;

        private const string MOVE = "Moving";
        private const string ATTACK = "Attack";
        private const string SPECIALCH = "SpecialCharge";
        private const string SPECIAL = "SpecialAttack";
        private const string HURT = "Hurt";
        private const string DIE = "Die";
            
        private static readonly int MovingAnim = Animator.StringToHash(MOVE);
        private static readonly int AttackAnim = Animator.StringToHash(ATTACK);
        private static readonly int SpecialChAnim = Animator.StringToHash(SPECIALCH);
        private static readonly int SpecialAnim = Animator.StringToHash(SPECIAL);
        private static readonly int HurtAnim = Animator.StringToHash(HURT);
        private static readonly int DieAnim = Animator.StringToHash(DIE);

        private bool specialTrigger;
        
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void Move(bool value)
        {
            anim.SetBool(MovingAnim, value);
            specialTrigger = true;
        }
        
        public void Attack()
        {
            anim.SetTrigger(AttackAnim);
            specialTrigger = true;
        }
        
        public void SpecialCharge(bool value)
        {
            if (specialTrigger)
            {
                specialTrigger = false;
                anim.SetTrigger(SpecialChAnim);
            }
            
            anim.SetBool(SpecialAnim, value);
        }
        
        public void Special(bool value)
        {
            anim.SetBool(SpecialAnim, value);
        }
        
        public void GetHurt()
        {
            anim.SetTrigger(HurtAnim);
            specialTrigger = true;
        }

        public void Die(bool value)
        {
            anim.SetBool(DieAnim, value);
            specialTrigger = false;
        }
    }
}