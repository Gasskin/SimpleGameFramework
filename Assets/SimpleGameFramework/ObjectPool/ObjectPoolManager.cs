using System;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using UnityEngine;

namespace SimpleGameFramework.ObjectPool
{
    public class ObjectPoolManager : ManagerBase
    {
        #region Implement

            public override int Priority 
            {
                get
                {
                    return ManagerPriority.ObjectPoolManager.GetHashCode();
                } 
            }
            
            public override void Init()
            {
                m_ObjectPools = new Dictionary<string, IObjectPool>();
            }

            public override void Update(float time)
            {
                foreach (IObjectPool objectPool in m_ObjectPools.Values)
                {
                    objectPool.Update(time);
                }

            }

            public override void ShutDown()
            {
                foreach (IObjectPool objectPool in m_ObjectPools.Values)
                {
                    objectPool.Shutdown();
                }
                m_ObjectPools.Clear();
            }

        #endregion

        #region Field

            /// <summary>
            /// 默认对象池容量
            /// </summary>
            private const int DefaultCapacity = ManagerConfig.ObjectPoolConfig.DEFAULT_CAPACITY;
     
            /// <summary>
            /// 默认对象过期秒数
            /// </summary>
            private const float DefaultExpireTime = ManagerConfig.ObjectPoolConfig.DEFAULT_EXPIRETIME;
            
            /// <summary>
            /// 对象池字典
            /// </summary>
            private Dictionary<string, IObjectPool> m_ObjectPools;

        #endregion

        #region Property

        /// <summary>
        /// 对象池数量
        /// </summary>
        public int Count
        {
            get { return m_ObjectPools.Count; }
        }

        #endregion

        #region 对象池管理

            /// <summary>
            /// 检查对象池
            /// </summary>
            public bool HasObjectPool<T>() where T : ObjectBase
            {
                return m_ObjectPools.ContainsKey(typeof(T).FullName);
            }
            
            /// <summary>
            /// 创建对象池
            /// </summary>
            public ObjectPool<T> CreateObjectPool<T>(int capacity = DefaultCapacity, float exprireTime = DefaultExpireTime, bool allowMultiSpawn = false) where T : ObjectBase
            {
                string name = typeof(T).FullName;
                if (HasObjectPool<T>())
                {
                    Debug.LogError("要创建的对象池已存在");
                    return null;
                }
                ObjectPool<T> objectPool = new ObjectPool<T>(name, 0, 2, allowMultiSpawn);
                m_ObjectPools.Add(name, objectPool);
                return objectPool;
            }
 
            /// <summary>
            /// 获取对象池
            /// </summary>
            public ObjectPool<T> GetObjectPool<T>() where T : ObjectBase
            {
                IObjectPool objectPool = null;
                m_ObjectPools.TryGetValue(typeof(T).FullName, out objectPool);
                return objectPool as ObjectPool<T>;
            }
 
            /// <summary>
            /// 销毁对象池
            /// </summary>
            public bool DestroyObjectPool<T>()
            {
                IObjectPool objectPool = null;
                if (m_ObjectPools.TryGetValue(typeof(T).FullName, out objectPool))
                {
                    objectPool.Shutdown();
                    return m_ObjectPools.Remove(typeof(T).FullName);
                }
 
                return false;
            }
            
            /// <summary>
            /// 释放所有对象池中的可释放对象。
            /// </summary>
            public void Release()
            {
                foreach (IObjectPool objectPool in m_ObjectPools.Values)
                {
                    objectPool.Release();
                }
            }
 
            /// <summary>
            /// 释放所有对象池中的未使用对象。
            /// </summary>
            public void ReleaseAllUnused()
            {
                foreach (IObjectPool objectPool in m_ObjectPools.Values)
                {
                    objectPool.ReleaseAllUnused();
                }
            }

        #endregion        
    }
}
