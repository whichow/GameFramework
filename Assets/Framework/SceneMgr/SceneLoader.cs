
using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoSingleton<SceneLoader>
{
    public float LoadingProgress
    {
        get
        {
            if(_asyncOp == null)
            {
                return 0;
            }
            else
            {
                if(_asyncOp.progress < 0.9f)
                {
                    return _asyncOp.progress;
                }
                else
                {
                    return 1f;
                }
            }
            // return _asyncOp != null ? _asyncOp.progress : 0;
        }
    }

    public bool IsDone
    {
        get
        {
            return _asyncOp != null ? _asyncOp.isDone : false;
        }
    }

    private AsyncOperation _asyncOp;

    /// <summary>
    /// 加载一个场景
    /// </summary>
    /// <param name="path">场景路径</param>
    public void LoadScene(string path)
    {
        SceneManager.LoadScene(path);
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="path">场景路径</param>
    /// <param name="onProgress">加载进度回调</param>
    /// <param name="onFinish">完成时的回调</param>
    public void LoadSceneAsync(string path)
    {
        _asyncOp = SceneManager.LoadSceneAsync(path);
    }
}
