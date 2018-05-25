using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    private List<IDataPersister> _dataList;
    private Queue<IDataPersister> _waitToSave;
    private Dictionary<string, object> _dataMap;

    void Awake()
    {
        _Init();
    }

    private void _Init()
    {
        _dataList = new List<IDataPersister>();
        _waitToSave = new Queue<IDataPersister>();
        _dataMap = new Dictionary<string, object>();
    }

    public void Register(IDataPersister data)
    {
        _dataList.Add(data);
        if(_dataMap.ContainsKey(data.Key))
        {
            data.LoadData(_dataMap[data.Key]);
        }
    }

    public void Remove(IDataPersister data)
    {
        _dataList.Remove(data);
    }

    void Update()
    {
        for (int i = 0; i < _dataList.Count; i++)
        {
            if (_dataList[i].IsDirty())
            {
                _waitToSave.Enqueue(_dataList[i]);
            }
        }

        while (_waitToSave.Count > 0)
        {
            var data = _waitToSave.Dequeue();
            _dataMap[data.Key] = data.SaveData();
        }
    }
}
