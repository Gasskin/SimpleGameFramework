using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleGameFramework.Bundle
{
    public class SingleABLoader:IDisposable
    {

        #region Field

        private AssetLoader assetLoader;

        #endregion

        #region Delegate
        

        #endregion
        
        #region 生命周期

        public SingleABLoader(string relativePath)
        {
            // 绝对路径
            var absolutePath = GetBundlePath(relativePath);
            // 加载AssetLoader
            var bundle = AssetBundle.LoadFromFile(absolutePath);
            assetLoader = new AssetLoader(bundle);
        }

        public void Dispose()
        {
            if (assetLoader != null) 
            {
                assetLoader.Dispose();
                assetLoader = null;
            }
            else
            {
                Debug.LogError("AssetLoader还未初始化");
            }
        }

        public void DisposeAll()
        {
            if (assetLoader != null) 
            {
                assetLoader.DisposeAll();
                assetLoader = null;
            }
            else
            {
                Debug.LogError("AssetLoader还未初始化");
            }
        }

        #endregion

        #region 接口

        /// <summary>
        /// 加载资源，本质是对AssetLoader的代理
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="cached">是否缓存</param>
        /// <typeparam name="T">资源类型</typeparam>
        /// <returns></returns>
        public T LoadAsset<T>(string assetName, bool cached = false) where T : UnityEngine.Object
        {
            return assetLoader.LoadAsset<T>(assetName, cached) as T;
        }

        /// <summary>
        /// 卸载资源，本质是对AssetLoader的代理
        /// </summary>
        /// <param name="asset">资源</param>
        /// <returns>是否卸载成功</returns>
        public bool UnLoadAsset(Object asset)
        {
            if (assetLoader != null)
            {
                assetLoader.UnLoadAsset(asset);
                return true;
            }
            Debug.LogError("AssetLoader还未初始化");
            return false;
        }

        #endregion

        #region 工具

        /// <summary>
        /// 根据Bundle的名称获取相应规则下Bundle的路径
        /// </summary>
        /// <param name="bundleName">Bundle名称</param>
        /// <returns>Bundle路径</returns>
        private string GetBundlePath(string bundleName)
        {
            return BundleUtils.GetPackageToLocation() + $"/{bundleName}";
        }

        #endregion
    }
}
