using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseState
{
    private Player _player;

    public WalkState(Player player) : base(player)
    {
        _player = player;
    }

    public override void OnEnter(IState preState)
    {
        Debug.Log("Enter Walk");
        _player.animator.SetBool("IsWalking", true);
        _player.Walk();
    }

    public override void OnExit(IState nextState)
    {
        Debug.Log("Exit Walk");
    }

    public override void OnUpdate()
    {
        if(_player.step <= 0)
        {
            _player.ChangeState(_player.idleState);
        }

        if(_player.health <= 0)
        {
            _player.ChangeState(_player.dieState);
        }
    }
}
