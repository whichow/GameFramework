using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : SceneState
{
	private SceneState _loadScene;

    public LoadingScene(SceneState loadScene)
    {
        SceneName = "Loading";
        _loadScene = loadScene;
    }

	public override void OnLoad()
	{
		SceneController.Instance.ChangeScene(_loadScene);
	}
}
