using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    public class ResInfo
    {
        public ResourceID resName;
        public string fullPath;
        public bool keepInMemory;
        public ResLoadFinished onLoadFinish;
        public ResourceRequest request;

        /// <summary>
        /// 资源是否加载完成
        /// </summary>
        public bool IsDone
        {
            get
            {
                return (request != null && request.isDone);
            }
        }

        /// <summary>
        /// 加载到的资源
        /// </summary>
        public Object Asset
        {
            get
            {
                return request != null ? request.asset : null;
            }
        }

        public void LoadAsync()
        {
            request = Resources.LoadAsync(fullPath);
        }
    }

    public class AssetInfo
    {
        public ResourceID resName;
        public Object asset;
        public bool keepInMemory;
    }

    public delegate void ResLoadFinished(Object res);

    //同时加载数
    public int AsyncLimit
    {
        get;
        set;
    }
    //最大缓存数
    private int CacheLimit
    {
        get;
        set;
    }
    private Dictionary<ResourceID, string> _resDic;
    //缓存的资源
    private List<AssetInfo> _cachedAssets;
    //等待加载的资源队列
    private Queue<ResInfo> _waitToLoad;
    //正在加载的资源
    private List<ResInfo> _loading;

    void Awake()
    {
        _Init();
        _InitRes();
    }

    private void _Init()
    {
        AsyncLimit = 1;
        CacheLimit = 20;

        _resDic = new Dictionary<ResourceID, string>();
        _cachedAssets = new List<AssetInfo>();
        _waitToLoad = new Queue<ResInfo>();
        _loading = new List<ResInfo>();
    }

    private void _InitRes()
    {
        var resList = Resources.Load<ResourceList>("ResourceList");
        foreach(var res in resList.resources)
        {
            _resDic.Add(res.resName, res.fullPath);
        }
    }

    /// <summary>
    /// 获取资源路径
    /// </summary>
    /// <param name="resName">资源ID</param>
    /// <returns>资源路径</returns>
    private string GetFullPath(ResourceID resName)
    {
        return _resDic[resName];
    }

    /// <summary>
    /// 加入缓存
    /// </summary>
    /// <param name="assetInfo">资源信息</param>
    private void AddToCache(AssetInfo assetInfo)
    {
        while(_cachedAssets.Count > CacheLimit - 1)
        {
            _cachedAssets.RemoveAt(0);
        }
        _cachedAssets.Add(assetInfo);
    }

    /// <summary>
    /// 获取资源信息
    /// </summary>
    /// <param name="resName">资源ID</param>
    /// <returns></returns>
    private AssetInfo GetAssetInfo(ResourceID resName)
    {
        for (int i = 0; i < _cachedAssets.Count; i++)
        {
            var assetInfo = _cachedAssets[i];
            if (assetInfo.resName == resName)
            {
                return assetInfo;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="resName">资源ID</param>
    /// <returns></returns>
    public Object GetAsset(ResourceID resName)
    {
        var assetInfo = GetAssetInfo(resName);
        if(assetInfo != null)
        {
            return assetInfo.asset;
        }
        return null;
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="resName">资源ID</param>
    /// <returns>加载的资源</returns>
    public Object Load(ResourceID resName, bool keepInMemory = false)
    {
        var assetInfo = GetAssetInfo(resName);
        if (assetInfo != null)
        {
            assetInfo.keepInMemory = keepInMemory;
            return assetInfo.asset;
        }
        var fullPath = GetFullPath(resName);
        var res = Resources.Load(fullPath);
        var newAssetInfo = new AssetInfo()
        {
            resName = resName,
            asset = res,
            keepInMemory = keepInMemory
        };
        AddToCache(newAssetInfo);
        return Resources.Load(fullPath);
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="resName">资源ID</param>
    /// <param name="loadFinishCallback">加载完成回调</param>
    public void LoadAsync(ResourceID resName, ResLoadFinished loadFinishCallback, bool keepInMemory = false)
    {
        var assetInfo = GetAssetInfo(resName);
        if (assetInfo != null)
        {
            assetInfo.keepInMemory = keepInMemory;
            loadFinishCallback(assetInfo.asset);
        }
        var resInfo = new ResInfo()
        {
            resName = resName,
            fullPath = GetFullPath(resName),
            keepInMemory = keepInMemory,
            onLoadFinish = loadFinishCallback
        };
        _waitToLoad.Enqueue(resInfo);
    }

    /// <summary>
    /// 清理所有非keepInMemory缓存资源
    /// </summary>
    public void RemoveAll()
    {
        for (int i = 0; i < _cachedAssets.Count; i++)
        {
            var assetInfo = _cachedAssets[i];
            if (!assetInfo.keepInMemory)
            {
                _cachedAssets.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 移除指定资源
    /// </summary>
    /// <param name="resName">资源ID</param>
    public void Remove(ResourceID resName)
    {
        for (int i = 0; i < _cachedAssets.Count; i++)
        {
            var assetInfo = _cachedAssets[i];
            if (assetInfo.resName == resName)
            {
                _cachedAssets.RemoveAt(i);
                break;
            }
        }
    }

    /// <summary>
    /// 释放所有非keepInMemory资源，并调用GC
    /// 注意：会造成卡顿，谨慎使用
    /// </summary>
    public void ReleaseAll()
    {
        RemoveAll();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    void Update()
    {
        while (_loading.Count < AsyncLimit && _waitToLoad.Count > 0)
        {
            var resInfo = _waitToLoad.Dequeue();
            _loading.Add(resInfo);
            resInfo.LoadAsync();
        }

        for (int i = 0; i < _loading.Count; i++)
        {
            if (_loading[i].IsDone)
            {
                ResInfo resInfo = _loading[i];
                _loading.RemoveAt(i);
                AssetInfo assetInfo = new AssetInfo()
                {
                    resName = resInfo.resName,
                    asset = resInfo.Asset,
                    keepInMemory = resInfo.keepInMemory
                };
                AddToCache(assetInfo);
                resInfo.onLoadFinish(resInfo.Asset);
            }
        }
    }
}
