using System;
using SimpleGameFramework.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleGameFramework.Resource.Bundle
{
    public class AssetLoader
    {
        #region Field

        /// <summary>
        /// 资源
        /// </summary>
        private Object asset;

        /// <summary>
        /// 当前资源是从哪个Bundle中加载出来的
        /// </summary>
        public string fromWitchBundle;
        
        #endregion

        #region Property

        /// <summary>
        /// 资源的引用计数，不返回资源本身，而是返回一个资源的实例（拷贝）
        /// </summary>
        public int AssetRefCount { get; set; }

        #endregion
        
        #region 生命周期

        public AssetLoader(string bundle, Object asset)
        {
            fromWitchBundle = bundle;
            AssetRefCount = 0;
            this.asset = asset;
        }
        
        public void ShutDown()
        {
            if (asset == null) 
                return;
            AssetRefCount = 0;
            Resources.UnloadAsset(asset);
            asset = null;
        }

        #endregion

        #region 接口

        /// <summary>
        /// 获取当前AssetLoader中保存的资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns>资源</returns>
        public T Acquire<T>() where T : Object
        {
            if (asset == null) 
            {
                Debug.LogError("想要获取的资源位空...");
                return null;
            }
            AssetRefCount++;
            var inst = Object.Instantiate(asset);

            var assetManager = SGFEntry.Instance.GetManager<AssetManager>();
            assetManager.CacheAssetLoader(inst, this);
            
            return inst as T;
        }

        public void Release(Object obj)
        {
            GameObject.Destroy(obj);
            AssetRefCount--;

            if (AssetRefCount < 1) 
            {
                var assetManager = SGFEntry.Instance.GetManager<AssetManager>();
                if (assetManager.bundleLoaded.TryGetValue(fromWitchBundle, out var bundleLoader)) 
                {
                    bundleLoader.Release();
                }
            }
        }

        #endregion
    }
}
