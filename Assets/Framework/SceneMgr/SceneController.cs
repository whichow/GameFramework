using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoSingleton<SceneController>
{
    public SceneState CurrentScene
    {
        get;
        private set;
    }
    private SceneState _loadState;

    void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnload;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        CurrentScene = _loadState;
        if (_loadState != null)
        {
            Debug.Log("Load: " + _loadState.SceneName);
            _loadState.OnLoad();
        }
    }

    private void OnSceneUnload(Scene scene)
    {
        if (CurrentScene != null)
        {
            Debug.Log("Unload: " + CurrentScene.SceneName);
            CurrentScene.OnUnload();
        }
        CurrentScene = null;
    }

    public void ChangeScene(SceneState state)
    {
        _loadState = state;
        SceneLoader.Instance.LoadSceneAsync(state.SceneName);
    }
}
