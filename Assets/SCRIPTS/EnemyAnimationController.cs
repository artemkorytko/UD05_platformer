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
        // передает в аниматора величину Move, получив из аниматор контроллера врага цифру 1 или -1
    }
}
