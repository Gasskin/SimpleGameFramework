using System;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleGameFramework.Resource.Bundle
{
    public class BundleLoader
    {

    #region Field

    /// <summary>
    /// 一个BundlerLoader对应一个AssetBundle
    /// </summary>
    private AssetBundle assetBundle;

    /// <summary>
    /// 每一个AssetBundle内部有多个资源
    /// </summary>
    private Dictionary<string, AssetLoader> assetCached;

    /// <summary>
    /// AssetBundle的引用计数，每有一个资源被加载，引用计数+1
    /// </summary>
    private int bundleRefCount;

    /// <summary>
    /// 当前Loader所加载的Bundle名称
    /// </summary>
    private string bundleName;

    #endregion

    #region Property

    public int BundleRefCount
    {
        get
        {
            return bundleRefCount;
        }
        private set
        {
            bundleRefCount = value;
        }
    }

    #endregion

    #region 生命周期

    public BundleLoader(BundleInfo bundleInfo)
    {
        // 初始化引用计数，加载Bundle并不会增加引用计数，必须LoadAsset才会
        BundleRefCount = 0;
        assetCached = new Dictionary<string, AssetLoader>();
        bundleName = bundleInfo.bundlePath;
        // 递归加载Bundle
        LoadBundleAndDependencies(bundleInfo);
    }


    public void ShutDown()
    {
        // 卸载Bundle和对应加载的资源
        assetBundle.Unload(true);
        
        // 清空缓存的资源
        foreach (var obj in assetCached)
        {
            obj.Value.ShutDown();
        }
        assetCached.Clear();
    }

    #endregion

    #region 接口

    /// <summary>
    /// 加载Bundle中的指定资源
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <typeparam name="T">资源类型</typeparam>
    public T LoadAsset<T>(string assetName) where T : Object
    {
        // 引用计数增加
        BundleRefCount++;
        // 如果已经加载过了，直接返回引用
        if (assetCached.ContainsKey(assetName))
        {
            return assetCached[assetName].Acquire<T>();
        }
        // 否则是第一次加载，需要从Bundle中加载
        var asset = assetBundle.LoadAsset<T>(assetName);
        if (asset == null) 
        {
            Debug.LogError($"加载资源失败..请检查{assetBundle.name}:{assetName}");
            return null;
        }
        else
        {
            AssetLoader assetLoader = new AssetLoader(bundleName, asset);
            assetCached.Add(assetName, assetLoader);
            return assetLoader.Acquire<T>();
        }
    }

    public void Release()
    {
        BundleRefCount--;
    }

    #endregion

    #region 工具

    /// <summary>
    /// 递归加载Bundle和其依赖，如果依赖的Bundle还有依赖，则会一直递归加载直到加载完所有的依赖包
    /// </summary>
    /// <param name="bundleInfo"></param>
    public void LoadBundleAndDependencies(BundleInfo bundleInfo)
    {
        // 说明有依赖
        if (bundleInfo.dependencies != null)
        {
            foreach (var bundle in bundleInfo.dependencies)
            {
                BundleLoader bundleLoader = new BundleLoader(bundle);
            }
        }

        // 当依赖都加载完毕后，继续加载Bundle本身
        var assetManager = SGFEntry.Instance.GetManager<AssetManager>();
        // 是否已经加载过当前Bundle
        if (assetManager.IsBundleLoaded(bundleInfo.bundlePath))
            return;

        var fullPath = BundleUtils.GetFullPath(bundleInfo.bundlePath);
        assetBundle = AssetBundle.LoadFromFile(fullPath);
        if (assetBundle == null)
        {
            throw new Exception($"Bundle加载错误，请检查路径：{fullPath}");
        }
        else
        {
            bundleName = bundleInfo.bundlePath;
            assetManager.CacheBundleLoader(bundleInfo.bundlePath, this);
        }
    }

    #endregion

    }
}
