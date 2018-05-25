using UnityEngine;

public class SceneState
{
    public string SceneName
    {
        get;
        protected set;
    }

    public virtual void OnLoad()
    {

    }

    public virtual void OnUnload()
    {

    }
}
