using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[System.Serializable]
public class StateMachine
{
    public State defaultState;

    private State _currentState;

    public void Process()
    {
        if(_currentState == null)
        {
            _currentState = defaultState;
        }
        var newState = _currentState.ProcessState();
        ChangeState(newState);
    }


    private void ChangeState(State newState)
    {
        // Switchin state if new state is not same as currentState
        if (_currentState != newState)
        {
            _currentState.StopState();
            newState.StartState();
            _currentState = newState;
        }
    }

    internal string GetCurrentStateName()
    {
        return _currentState.GetType().Name;
    }
}
