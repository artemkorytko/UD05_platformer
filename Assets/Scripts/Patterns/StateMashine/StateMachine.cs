using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class StateMachine : MonoBehaviour // мммммм-м х#ет@
    {

        private State _firstState;
        private State _currentState; // текущее состояние

        private void Start()
        {
            SetState(_firstState);
        }

        private void Update()
        {
            throw new NotImplementedException();
        }


        private void SetState(State state) // установить состояние
        {
            _currentState = state;
            if(_currentState != null)
                _currentState.Enter();
        }

        private void TransitionInState(State nextState) // перейти в след состояние
        {
            _currentState = nextState;
        }
    }
}