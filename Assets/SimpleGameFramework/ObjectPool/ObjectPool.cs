using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework.ObjectPool
{
    public class ObjectPool<T> : IObjectPool where T : ObjectBase
    {
        #region Implement

            public string Name { get; private set; }
            
            public Type ObjectType
            {
                get { return typeof(T); }
            }
            
            public int Count
            {
                get { return m_Objects.Count; }
            }
            
            public int CanReleaseCount {
                get
                {
                    return GetCanReleaseObjects().Count;
                }
            }
            
            public float AutoReleaseInterval { get; set; }

            public int Capacity
            {
                get { return m_Capacity; }
                set
                {
                    if (value < 0)
                        Debug.LogError("对象池容量必须大于0");

                    if (m_Capacity == value) 
                        return;

                    m_Capacity = value;
                }
            }

            public float ExpireTime
            {
                get { return m_ExpireTime; }

                set
                {
                    if (value < 0)
                        Debug.LogError("对象过期秒数必须大于0");

                    if (m_ExpireTime == value)
                        return;

                    m_ExpireTime = value;
                }
            }

            /// <summary>
            /// 释放超出对象池容量的可释放对象
            /// </summary>
            public void Release()
            {
                Release(m_Objects.Count - m_Capacity, DefaultReleaseObjectFilterCallBack);
            }
 
            /// <summary>
            /// 释放指定数量的可释放对象
            /// </summary>
            /// <param name="toReleaseCount"></param>
            public void Release(int toReleaseCount)
            {
                Release(toReleaseCount, DefaultReleaseObjectFilterCallBack);
            }
 
            /// <summary>
            /// 释放对象池中所有未使用对象
            /// </summary>
            public void ReleaseAllUnused()
            {
                LinkedListNode<ObjectBase> current = m_Objects.First;
                while (current != null)
                {
                    if (current.Value.IsInUse)
                    {
                        current = current.Next;
                        continue;
                    }
 
                    LinkedListNode<ObjectBase> next = current.Next;
                    m_Objects.Remove(current);
                    current.Value.Release();
                    Debug.Log("对象被释放了：" + current.Value.Name);
                    current = next;
                }
            }

            /// <summary>
            /// 对象池的定时释放
            /// </summary>
            public void Update(float time)
            {
                AutoReleaseTime += time;
                if (AutoReleaseTime < AutoReleaseInterval)
                    return;
 
                Release();
            }
 
            /// <summary>
            /// 清理对象池
            /// </summary>
            public void Shutdown()
            {
                LinkedListNode<ObjectBase> current = m_Objects.First;
                while (current != null)
                {
 
                    LinkedListNode<ObjectBase> next = current.Next;
                    m_Objects.Remove(current);
                    current.Value.Release();
                    Debug.Log("对象被释放了：" + current.Value.Name);
                    current = next;
                }
            }

        #endregion

        #region Delegate

        /// <summary>
        /// 释放对象筛选方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="candidateObjects">要筛选的对象集合</param>
        /// <param name="toReleaseCount">需要释放的对象数量</param>
        /// <param name="expireTime">对象过期参考时间</param>
        /// <returns>经筛选需要释放的对象集合</returns>
        public delegate LinkedList<T> ReleaseObjectFilterCallback<T>(LinkedList<T> candidateObjects, int toReleaseCount, DateTime expireTime) where T : ObjectBase;

        /// <summary>
        /// 默认的释放对象筛选方法（未被使用且过期的对象）
        /// </summary>
        private LinkedList<T> DefaultReleaseObjectFilterCallBack(LinkedList<T> candidateObjects, int toReleaseCount, DateTime expireTime)
        {
            LinkedList<T> toReleaseObjects = new LinkedList<T>();
 
            if (expireTime > DateTime.MinValue)
            {
                LinkedListNode<T> current = candidateObjects.First;
                while (current != null)
                {
                    // 对象最后使用时间 <= 过期参考时间，就需要释放
                    if (current.Value.LastUseTime <= expireTime)
                    {
                        toReleaseObjects.AddLast(current.Value);
                        LinkedListNode<T> next = current.Next;
                        candidateObjects.Remove(current);
                        toReleaseCount--;
                        if (toReleaseCount <= 0)
                        {
                            return toReleaseObjects;
                        }
                        current = next;
                        continue;
                    }
 
                    current = current.Next;
                }
 
            }
 
            return toReleaseObjects;
        }
        
        #endregion

        #region Field

            /// <summary>
            /// 对象池容量
            /// </summary>
            private int m_Capacity;
         
            /// <summary>
            /// 对象池对象过期秒数
            /// </summary>
            private float m_ExpireTime;
             
            /// <summary>
            /// 池对象的链表
            /// </summary>
            private LinkedList<ObjectBase> m_Objects;

        #endregion
        
        #region Property
         
            /// <summary>
            /// 池对象是否可被多次获取
            /// </summary>
            public bool AllowMultiSpawn { get; private set; }
         
            /// <summary>
            /// 对象池自动释放可释放对象计时
            /// </summary>
            public float AutoReleaseTime { get; private set; }

        #endregion

        #region 生命周期

            public ObjectPool(string name, int capacity, float expireTime, bool allowMultiSpawn)
            {
                Name = name;
                m_Objects = new LinkedList<ObjectBase>();
     
                Capacity = capacity;
                AutoReleaseInterval = expireTime; 
                ExpireTime = expireTime;
                AutoReleaseTime = 0f;
                AllowMultiSpawn = allowMultiSpawn;
            }

        #endregion

        #region 对象的注册、获取、归还

            /// <summary>
            /// 检查对象
            /// </summary>
            /// <param name="name">对象名称</param>
            /// <returns>要检查的对象是否存在</returns>
            public bool CanSpawn(string name)
            {
                foreach (ObjectBase obj in m_Objects)
                {
                    if (obj.Name != name)
                    {
                        continue;
                    }
     
                    if (AllowMultiSpawn || !obj.IsInUse)
                    {
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// 注册对象
            /// </summary>
            /// <param name="obj">对象</param>
            /// <param name="spawned">对象是否已被获取</param>
            public void Register(T obj, bool spawned = false)
            {
                if (obj == null)
                {
                    Debug.LogError("要放入对象池的对象为空:" + typeof(T).FullName);
                    return;
                }
                // 已被获取就让计数+1
                if (spawned)
                {
                    obj.SpawnCount++;
                }
                m_Objects.AddLast(obj);
            }
 
            /// <summary>
            /// 获取对象
            /// </summary>
            /// <param name="name">对象名称</param>
            /// <returns>要获取的对象</returns>
            public T Spawn(string name = "")
            {
                foreach (ObjectBase obj in m_Objects)
                {
                    if (obj.Name != name)
                    {
                        continue;
                    }
 
                    if (AllowMultiSpawn || !obj.IsInUse)
                    {
                        Debug.Log("获取了对象：" + typeof(T).FullName + "/" + obj.Name);
                        return obj.Spawn() as T;
                    }
                }
                return null;
            }
 
            /// <summary>
            /// 回收对象
            /// </summary>
            public void Unspawn(ObjectBase obj)
            {
                Unspawn(obj.Target);
            }
 
            /// <summary>
            /// 回收对象
            /// </summary>
            public void Unspawn(object target)
            {
                if (target == null)
                    Debug.LogError("要回收的对象为空：" + typeof(object).FullName);
 
                foreach (ObjectBase obj in m_Objects)
                {
                    if (obj.Target == target)
                    {
                        obj.Unspawn();
                        Debug.Log("对象被回收了：" + typeof(T).FullName + "/" + obj.Name);
                        return;
                    }
                }
 
                Debug.LogError("找不到要回收的对象：" + typeof(object).FullName);
            }
            
        #endregion

        #region 对象的释放

        /// <summary>
            /// 获取所有可以释放的对象
            /// </summary>
            private LinkedList<T> GetCanReleaseObjects()
            {
                LinkedList<T> canReleaseObjects = new LinkedList<T>();
 
                foreach (ObjectBase obj in m_Objects)
                {
                    if (obj.IsInUse)
                    {
                        continue;
                    }
 
                    canReleaseObjects.AddLast(obj as T);
                }
 
                return canReleaseObjects;
            }
            
            /// <summary>
            /// 释放对象池中的可释放对象
            /// </summary>
            /// <param name="toReleaseCount">尝试释放对象数量</param>
            /// <param name="releaseObjectFilterCallback">释放对象筛选方法</param>
            public void Release(int toReleaseCount, ReleaseObjectFilterCallback<T> releaseObjectFilterCallback)
            {
                // 重置计时
                AutoReleaseTime = 0;
 
                if (toReleaseCount <= 0)
                    return;
 
                // 计算对象过期参考时间
                DateTime expireTime = DateTime.MinValue;
                if (m_ExpireTime < float.MaxValue)
                    // 当前时间 - 过期秒数 = 过期参考时间
                    expireTime = DateTime.Now.AddSeconds(-m_ExpireTime);
 
                // 获取可释放的对象和实际要释放的对象
                LinkedList<T> canReleaseObjects = GetCanReleaseObjects();
                LinkedList<T> toReleaseObjects = releaseObjectFilterCallback(canReleaseObjects, toReleaseCount, expireTime);
                if (toReleaseObjects == null || toReleaseObjects.Count <= 0)
                    return;
 
                // 遍历实际要释放的对象
                foreach (ObjectBase toReleaseObject in toReleaseObjects)
                {
                    if (toReleaseObject == null)
                        Debug.LogError("无法释放空对象");
 
                    foreach (ObjectBase obj in m_Objects)
                    {
                        if (obj != toReleaseObject)
                            continue;
 
                        // 释放对象
                        m_Objects.Remove(obj);
                        obj.Release();
                        Debug.Log("对象被释放了：" + obj.Name);
                        break;
                    }
                }
 
            }

        #endregion
    }
}
