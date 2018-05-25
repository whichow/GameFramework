using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
	void Start()
    {
        SceneController.Instance.ChangeScene(new LoadingScene(new GameScene()));
    }
}
