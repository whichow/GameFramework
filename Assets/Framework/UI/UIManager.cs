using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private Transform _uiRoot;
    private Stack<UIView> _popupStack;
    private UIView _openingView;
    private Dictionary<UIName, UIView> _viewMap;

    void Awake()
    {
        _Init();
    }

    private void _Init()
    {
        _popupStack = new Stack<UIView>();
        _viewMap = new Dictionary<UIName, UIView>();

        _uiRoot = GameObject.Find("RootCanvas").transform;
    }

    /// <summary>
    /// 添加一个UIView到UIManager中
    /// </summary>
    /// <param name="view">UIView</param>
    public void AddView(UIView view)
    {
        if (!_viewMap.ContainsKey(view.viewName))
        {
            _viewMap.Add(view.viewName, view);
            view.transform.SetParent(_uiRoot, false);
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
    /// 打开一个UIView
    /// </summary>
    /// <param name="name">UI名</param>
    public void OpenView(UIName name)
    {
        var view = GetView(name);
        if (view != null)
        {
            OpenView(view);
        }
    }

    /// <summary>
    /// 关闭一个UIView
    /// </summary>
    /// <param name="name">UI名</param>
    public void CloseView(UIName name)
    {
        var view = GetView(name);
        if (view != null)
        {
            CloseView(view);
        }
    }

    /// <summary>
    /// 打开一个UIView，如果ViewType为Popup，则放入Popup栈管理，
    /// 如果ViewType为Normal，则清空Popup栈并关闭其他Noraml类型的View
    /// </summary>
    /// <param name="view">UIView</param>
    public void OpenView(UIView view)
    {
        if (view.viewType == ViewType.Popup)
        {
            _popupStack.Push(view);
            view.Show();
        }
        else if (view.viewType == ViewType.Normal)
        {
            ClearPopupStack();
            if (_openingView != null)
            {
                _openingView.Hide();
            }
            view.Show();
            _openingView = view;
        }
        else
        {
            view.Show();
        }
    }

    /// <summary>
    /// 关闭一个UIView，如果ViewType为Popup，则该UIView必须要在栈顶
    /// </summary>
    /// <param name="view"></param>
    public void CloseView(UIView view)
    {
        if (view.viewType == ViewType.Popup)
        {
            if (_popupStack.Peek() == view)
            {
                _popupStack.Pop();
                view.Hide();
            }
        }
        else if (view.viewType == ViewType.Normal)
        {
            if (_openingView == view)
            {
                ClearPopupStack();
                view.Hide();
                _openingView = null;
            }
        }
        else
        {
            view.Hide();
        }
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
