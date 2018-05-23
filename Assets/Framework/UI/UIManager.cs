using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private Transform _uiRoot;
    private Stack<UIView> _popupStack;
    private Dictionary<ViewType, Transform> _layerMap;
    private Dictionary<UIName, UIView> _viewMap;

    void Awake()
    {
        _Init();
        _InitLayer();
    }

    private void _Init()
    {
        _popupStack = new Stack<UIView>();
        _layerMap = new Dictionary<ViewType, Transform>();
        _viewMap = new Dictionary<UIName, UIView>();
        
        _uiRoot = GameObject.Find("RootCanvas").transform;
    }

    private void _InitLayer()
    {
        ViewType[] viewTypes = (ViewType[])System.Enum.GetValues(typeof(ViewType));
        foreach(var type in viewTypes)
        {
            var go = new GameObject(type.ToString());
            go.transform.SetParent(_uiRoot, false);
            _layerMap[type] = go.transform;
        }
    }

    /// <summary>
    /// 从ResourceManager中加载一个UIView并添加到UIManager中
    /// </summary>
    /// <param name="resName">资源ID</param>
    public void LoadView(ResourceID resName)
    {
        ResourceManager.Instance.LoadAsync(resName, (res) =>
        {
            var go = res as GameObject;
            var view = go.GetComponent<UIView>();
            AddView(view);
        });
    }

    /// <summary>
    /// 添加一个UIView到UIManager中
    /// </summary>
    /// <param name="view">UIView</param>
    public void AddView(UIView view)
    {
        if (_viewMap.ContainsKey(view.viewName))
        {
            _viewMap.Add(view.viewName, view);
            AddToLayer(view);
        }
    }

    /// <summary>
    /// 获取一个UIView
    /// </summary>
    /// <param name="name">UI名</param>
    /// <returns>UIView</returns>
    public UIView GetView(UIName name)
    {
        UIView view;
        _viewMap.TryGetValue(name, out view);
        return view;
    }

    /// <summary>
    /// 显示一个UIView
    /// </summary>
    /// <param name="name">UI名</param>
    public void ShowView(UIName name)
    {
        var view = GetView(name);
        if (view != null)
        {
            ShowView(view);
        }
    }

    /// <summary>
    /// 隐藏一个UIView
    /// </summary>
    /// <param name="name">UI名</param>
    public void HideView(UIName name)
    {
        var view = GetView(name);
        if (view != null)
        {
            HideView(view);
        }
    }

    /// <summary>
    /// 显示一个UIView，如果ViewType为Popup，则放入Popup栈管理，
    /// 如果ViewType为Normal，则清空Popup栈
    /// </summary>
    /// <param name="view">UIView</param>
    public void ShowView(UIView view)
    {
        if (view.viewType == ViewType.Popup)
        {
            _popupStack.Push(view);
            view.Show();
        }
        if (view.viewType == ViewType.Normal)
        {
            ClearPopupStack();
            view.Show();
        }
        if (view.viewType == ViewType.Fixed)
        {
            view.Show();
        }
    }

    /// <summary>
    /// 隐藏一个UIView，如果ViewType为Popup，则该UIView必须要在栈顶
    /// </summary>
    /// <param name="view"></param>
    public void HideView(UIView view)
    {
        if (view.viewType == ViewType.Popup)
        {
            if (_popupStack.Peek() == view)
            {
                _popupStack.Pop();
                view.Hide();
            }
        }
        else
        {
            view.Hide();
        }
    }

    /// <summary>
    /// 将UIView添加到相应层
    /// </summary>
    /// <param name="view"></param>
    private void AddToLayer(UIView view)
    {
        var layer = _layerMap[view.viewType];
        view.transform.SetParent(layer, false);
    }

    /// <summary>
    /// 清空Popup栈并隐藏栈中的UIView
    /// </summary>
    private void ClearPopupStack()
    {
        while (_popupStack.Count > 0)
        {
            var view = _popupStack.Pop();
            view.Hide();
        }
    }
}
