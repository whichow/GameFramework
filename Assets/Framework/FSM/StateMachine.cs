using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	private IState _currentState;

	public IState CurrentState
	{
		get
		{
			return _currentState;
		}
	}

	public void ChangeState(IState state)
	{
		if(_currentState != null)
		{
			_currentState.OnExit(state);
		}
		state.OnEnter(_currentState);
		_currentState = state;
	}

	void Update()
	{
		if(_currentState != null)
		{
			_currentState.OnUpdate();
		}
	}
}
