using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState
{
    private Player _player;

    public DieState(Player player) : base(player)
    {
        _player = player;
    }

    public override void OnEnter(IState preState)
    {
        Debug.Log("Enter Die");
        _player.animator.SetTrigger("Die");
    }

    public override void OnExit(IState nextState)
    {
        Debug.Log("Exit Die");
    }
}
