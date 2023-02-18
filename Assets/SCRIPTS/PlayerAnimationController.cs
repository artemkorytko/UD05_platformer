using System;
using UnityEngine;


// навесить на плеера!
namespace DefaultNamespace
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private const string SPEED = "Move";
        private const string JUMP = "JUMP"; //слова-кондишены копируем из аниматора

        // правой кнопкой (?) желтая лампочка
        private static readonly int Speed = Animator.StringToHash(SPEED);
        private static readonly int Jump = Animator.StringToHash(JUMP);
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        //----- методы вызывают события

        public void SetSpeed(int value)
        {
            // послали скорость - он там сам выбирает анимацию которая соответствует условием
           _animator.SetInteger(SPEED, value);
        }
        
        
        public void SetJump()
        {
            // Set обращается к AnyState у аниматора и тот уже по логике выбирает куда транзитить
            _animator.SetTrigger(JUMP);
        }
    }
}