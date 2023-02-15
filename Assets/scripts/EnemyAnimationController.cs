using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
  private const string MOVE = "Move";

  private Animator _animator;
  private static readonly int Move = Animator.StringToHash(MOVE);

  private void Awake()
  {
    _animator = GetComponent<Animator>();
    
  }

  public void SetMove(int value)
  {
    _animator.SetInteger(Move, value);
  }
}
