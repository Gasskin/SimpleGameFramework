using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework.ReferencePool
{
    public static class ReferencePool
    {
        #region Private

        /// 维护所有的引用集合，每一个集合内有多个同类型引用
        private static Dictionary<string, ReferenceCollection> s_ReferenceCollections = new Dictionary<string, ReferenceCollection>();

        #endregion

        #region Public

        public static int Count
        {
            get
            {
                return s_ReferenceCollections.Count;
            }
        }

        #endregion

        #region Public 接口方法

        /// 从引用集合获取引用
        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T).FullName).Acquire<T>();
        }
        
        /// 将引用归还引用集合
        public static void Release<T>(T reference) where T : class, IReference
        {
            if (reference == null)
            {
                throw new Exception("要归还的引用为空...");
            }
 
            GetReferenceCollection(typeof(T).FullName).Release(reference);
        }

        /// 清除所有引用集合
        public static void ClearAll()
        {
            lock (s_ReferenceCollections)
            {
                foreach (KeyValuePair<string, ReferenceCollection> referenceCollection in s_ReferenceCollections)
                {
                    referenceCollection.Value.RemoveAll();
                }
 
                s_ReferenceCollections.Clear();
            }
        }
        
        /// 从引用集合中移除所有的引用
        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceCollection(typeof(T).FullName).RemoveAll();
        }

        #endregion

        #region Private 工具方法

        /// 获取引用集合
        private static ReferenceCollection GetReferenceCollection(string fullName)
        {
            ReferenceCollection referenceCollection = null;
            lock (s_ReferenceCollections)
            {
                if (!s_ReferenceCollections.TryGetValue(fullName, out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection();
                    s_ReferenceCollections.Add(fullName, referenceCollection);
                }
            }
 
            return referenceCollection;
        }

        #endregion
    }
}