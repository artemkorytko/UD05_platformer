using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private const string SPEED = "Speed";
        private const string JUMP = "Jump";
        
        private static readonly int Jump = Animator.StringToHash(JUMP);
        private static readonly int Speed = Animator.StringToHash(SPEED);
        
        private Animator _animator;
        

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetSpeed(int value)
        {
            _animator.SetInteger(Speed, value);
        }

        public void SetJump()
        {
            _animator.SetTrigger(Jump);
        }
    }
}