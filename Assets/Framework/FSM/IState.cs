using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
	void OnEnter(IState preState);

	void OnExit(IState nextState);

	void OnUpdate();
}
