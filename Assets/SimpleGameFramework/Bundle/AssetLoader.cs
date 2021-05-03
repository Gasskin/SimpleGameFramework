using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SimpleGameFramework.Bundle
{
    public class AssetLoader : IDisposable
    {

        #region Field

        private AssetBundle currentAssetBundle;

        private Hashtable hashTable;

        #endregion

        #region 生命周期

        public AssetLoader(AssetBundle bundle)
        {
            currentAssetBundle = bundle;
            hashTable = new Hashtable();
        }
        
        public void Dispose()
        {
            currentAssetBundle.Unload(false);
        }

        public void DisposeAll()
        {
            currentAssetBundle.Unload(true);
        }

        #endregion

        #region 接口

        /// <summary>
        /// 加载Bundle中的Asset
        /// </summary>
        /// <param name="assetName">Asset名称</param>
        /// <param name="cahced">是否缓存</param>
        /// <returns>资源</returns>
        public Object LoadAsset<T>(string assetName, bool cahced = false) where T : Object
        {
            return LoadAssetInternal<T>(assetName, cahced);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="asset">资源</param>
        /// <returns>是否成功</returns>
        public bool UnLoadAsset(Object asset)
        {
            if (asset != null) 
            {
                Resources.UnloadAsset(asset);
                return true;
            }

            Debug.LogError("想要释放的资源为空");
            return false;
        }

        /// <summary>
        /// 查询Bundle中所有的Asset名称
        /// </summary>
        /// <returns>Asset名称数组</returns>
        public string[] RetriveAllAssetNames()
        {
            return currentAssetBundle.GetAllAssetNames();
        }

        #endregion

        #region 工具

        /// <summary>
        /// 加载资源的内部方法
        /// </summary>
        private T LoadAssetInternal<T>(string assetName,bool cached) where T : Object
        {
            if (hashTable.Contains(assetName))
            {
                return hashTable[assetName] as T;
            }
            else
            {
                var temp = currentAssetBundle.LoadAsset<T>(assetName);
                
                if (temp == null)
                    throw new Exception($"加载资源失败，请检查资源：{assetName}");
                
                if (cached)
                    hashTable.Add(assetName, temp);

                return temp;
            }
        }

        #endregion

    }
}
