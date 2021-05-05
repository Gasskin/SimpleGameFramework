using System;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.Resource.Bundle;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleGameFramework.Resource
{
    public class AssetManager : ManagerBase
    {
        #region Imepement

        public override int Priority
        {
            get
            {
                return ManagerPriority.AssetManager.GetHashCode();
            }
        }

        public override void Init()
        {
            bundleLoaded = new Dictionary<string, BundleLoader>();
            assetLoaded = new Dictionary<Object, AssetLoader>();
        }

        public override void Update(float time)
        {
            
        }

        public override void ShutDown()
        {
            foreach (var bundleLoader in bundleLoaded)
            {
                bundleLoader.Value.ShutDown();
            }

            foreach (var assetLoader in assetLoaded)
            {
                assetLoader.Value.ShutDown();
            }
            
            bundleLoaded.Clear();
            
            assetLoaded.Clear();
        }

        #endregion

        #region Field

        /// <summary>
        /// 已经加载过的Bundle的字典
        /// </summary>
        public Dictionary<string, BundleLoader> bundleLoaded;

        /// <summary>
        /// 资源的实例和其对应的Loader
        /// </summary>
        public Dictionary<Object, AssetLoader> assetLoaded;

        #endregion

        #region 接口

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>资源的一个实例，不会返回资源本身</returns>
        /// <exception cref="Exception">加载错误，没有正确加载Bundle包</exception>
        public T LoadAsset<T>(AssetInfo assetInfo) where T : Object
        {
            // 加载Bundle
            BundleLoader bundleLoader = new BundleLoader(assetInfo.bundle);
            if (bundleLoaded.TryGetValue(assetInfo.bundle.bundlePath, out var bundle))
            {
                return bundle.LoadAsset<T>(assetInfo.assetName);
            }
            else
            {
                throw new Exception($"加载资源错误：{assetInfo}");
            }
        }

        /// <summary>
        /// 释放一个资源，必须是通过AssetManger.LoadAsset加载的资源才可以通过这个接口释放
        /// </summary>
        /// <param name="obj"></param>
        public void Release(Object obj)
        {
            if (obj == null) 
                return;
            if (assetLoaded.TryGetValue(obj, out var assetLoader)) 
            {
                // 删除
                assetLoaded.Remove(obj);
                // 卸载
                assetLoader.Release(obj);
            }
            else
            {
                Debug.LogError("想要释放的资源没有加载...");
            }
        }

        #endregion

        #region 工具

        /// <summary>
        /// 是否加载过某个Bundle了
        /// </summary>
        /// <param name="bundleName">名称</param>
        /// <returns>是否加载过</returns>
        public bool IsBundleLoaded(string bundleName)
        {
            return bundleLoaded.ContainsKey(bundleName);
        }

        /// <summary>
        /// 缓存BundleLoader
        /// </summary>
        /// <param name="bundleName">名称</param>
        /// <param name="assetBundle">AssetBundle</param>
        public void CacheBundleLoader(string bundleName, BundleLoader bundleLoader)
        {
            if (IsBundleLoaded(bundleName))
                return;
            bundleLoaded.Add(bundleName, bundleLoader);
        }

        public void CacheAssetLoader(Object obj, AssetLoader assetLoader)
        {
            if (assetLoaded.ContainsKey(obj))
                return;
            assetLoaded.Add(obj, assetLoader);
        }


        #endregion
    }
}
