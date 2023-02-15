using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private const string MOVE = "Move";
    private static readonly int Move = Animator.StringToHash(MOVE);
    
    private Animator _animator;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMove(int value)
    {
        _animator.SetInteger(Move, value);
    }
}
