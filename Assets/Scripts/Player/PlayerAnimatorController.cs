using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private const string SPEED = "Speed";
        private const string JUMP = "Jump";

        private Animator _animaror;
        private static readonly int Speed = Animator.StringToHash(SPEED);
        private static readonly int Jump = Animator.StringToHash(JUMP);

        private void Awake()
        {
            _animaror = GetComponent<Animator>();
        }

        public void SetSpeed(int value)
        {
            _animaror.SetInteger(Speed, value);
        }

        public void SetJump()
        {
            _animaror.SetTrigger(Jump);
        }
    }
}