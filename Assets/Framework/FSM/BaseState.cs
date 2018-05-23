using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : IState
{
    protected StateMachine _fsm;

    public BaseState(StateMachine fsm)
    {
        _fsm = fsm;
    }

    public virtual void OnEnter(IState preState)
    {
        
    }

    public virtual void OnExit(IState nextState)
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }
}
