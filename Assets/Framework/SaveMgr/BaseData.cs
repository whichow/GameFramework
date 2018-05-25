using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData : MonoBehaviour, IDataPersister
{
    protected bool _isDirty;

    public string Key
    {
        get;
        protected set;
    }

    protected virtual void Awake()
    {
        DataManager.Instance.Register(this);
    }

    protected virtual void OnDestroy()
    {
        DataManager.Instance.Remove(this);
    }

    public virtual bool IsDirty()
    {
        return _isDirty;
    }

    public virtual void LoadData(object data)
    {

    }

    public virtual object SaveData()
    {
        return null;
    }
}
