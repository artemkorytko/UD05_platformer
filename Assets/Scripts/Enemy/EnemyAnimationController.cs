using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private const string MOVE = "Run";
        private static readonly int Run = Animator.StringToHash(MOVE);
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetMove(int value)
        {
            _animator.SetInteger(Run, value);
        }
    }
}