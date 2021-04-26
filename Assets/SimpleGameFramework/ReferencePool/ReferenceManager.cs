using System;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using UnityEngine;

namespace SimpleGameFramework.ReferencePool
{
    public class ReferenceManager : ManagerBase
    {
        #region Private

        /// 维护所有的引用集合，每一个集合内有多个同类型引用
        private Dictionary<string, ReferenceCollection> s_ReferenceCollections;

        /// 清理间隔，每过一段时间，就会清空队列里的引用（还在队列里，说明是空引用），m_temp用于实际计算，想要修改清理间隔可以修改clearInterval
        private float m_ClearInterval = ManagerConfig.ReferencePoolConfig.CLEAR_INTERVAL;
        private float m_Temp;
        
        #endregion

        #region Public

        public int Count
        {
            get
            {
                return s_ReferenceCollections.Count;
            }
        }

        #endregion

        #region 构造函数

        public ReferenceManager()
        {
            s_ReferenceCollections = new Dictionary<string, ReferenceCollection>();
        }

        #endregion

        #region Override

        public override int Priority
        {
            get
            {
                return ManagerPriority.ReferenceManager.GetHashCode();
            }
        }

        public override void Init()
        {
            m_Temp = m_ClearInterval;
        }

        public override void Update(float time)
        {
            m_Temp -= time;
            if (m_Temp < 0f) 
            {
#if UNITY_EDITOR
                var trans = SGFEntry.Instance.transform.Find("ReferenceManager");
                for (int i = 0; i < trans.childCount; i++)
                {
                    GameObject.DestroyImmediate(trans.GetChild(i).gameObject);
                }
#endif
                foreach (var e in s_ReferenceCollections)
                {
                    e.Value.RemoveAll();
                }

                m_Temp = m_ClearInterval;
            }
        }

        public override void ShutDown()
        {
            
        }
        
        #endregion

        #region Public 接口方法

        /// 从引用集合获取引用
        public  T Acquire<T>() where T : class, IReference, new()
        {
#if UNITY_EDITOR
            var trans = SGFEntry.Instance.transform.Find("ReferenceManager");
            var collection = trans.Find(typeof(T).FullName);
            if (collection == null)
            {
                GameObject go = new GameObject();
                go.name = typeof(T).FullName;
                go.transform.SetParent(trans);
            }
#endif
            return GetReferenceCollection(typeof(T).FullName).Acquire<T>();
        }
        
        /// 将引用归还引用集合
        public  void Release<T>(T reference) where T : class, IReference
        {
            if (reference == null)
            {
                throw new Exception("要归还的引用为空...");
            }

            GetReferenceCollection(typeof(T).FullName).Release(reference);
        }

        /// 清除所有引用集合
        public  void ClearAll()
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
        public  void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceCollection(typeof(T).FullName).RemoveAll();
        }

        #endregion

        #region Private 工具方法

        /// 获取引用集合
        private  ReferenceCollection GetReferenceCollection(string fullName)
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