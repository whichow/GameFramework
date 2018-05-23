using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    private Player _player;

    public IdleState(Player player) : base(player)
    {
        _player = player;
    }

    public override void OnEnter(IState preState)
    {
        Debug.Log("Enter Idle");
        _player.animator.SetBool("IsWalking", false);
    }

    public override void OnExit(IState nextState)
    {
        Debug.Log("Exit Idle");
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _player.ThrowDice();
            _player.ChangeState(_player.walkState);
        }
    }
}
