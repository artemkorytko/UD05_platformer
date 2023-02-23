using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator anim;
        
        private const string RUN = "Running";
        private const string CROUCH = "IsCrouching";
        private const string FLY = "isFlying";
        private const string JUMP = "Jump";
        private const string ATTACK = "Attack";
        private const string FLYATTACK = "FlyAttack";
        private const string TAKEDAMAGE = "TakeDamage";
        
        private static readonly int RunningAnim = Animator.StringToHash(RUN);
        private static readonly int CrouchingAnim = Animator.StringToHash(CROUCH);
        private static readonly int FlyingAnim = Animator.StringToHash(FLY);
        private static readonly int JumpAnim = Animator.StringToHash(JUMP);
        private static readonly int AttackAnim = Animator.StringToHash(ATTACK);
        private static readonly int FlyAttackAnim = Animator.StringToHash(FLYATTACK);
        private static readonly int TakeDamageAnim = Animator.StringToHash(TAKEDAMAGE);

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void SetRun(int value)
        {
            anim.SetInteger(RunningAnim, value);
        }
        
        public void SetCrouch(bool value)
        {
            anim.SetBool(CrouchingAnim, value);
        }

        public void SetFly(bool value)
        {
            anim.SetBool(FlyingAnim, value);
        }

        public void SetJump()
        {
            anim.SetTrigger(JumpAnim);
        }
        
        public void SetAttack()
        {
            anim.SetTrigger(AttackAnim);
        }
        
        public void SetFlyAttack()
        {
            anim.SetTrigger(FlyAttackAnim);
        }
        
        public void SetTakeDamage()
        {
            anim.SetTrigger(TakeDamageAnim);
        }
    }
}