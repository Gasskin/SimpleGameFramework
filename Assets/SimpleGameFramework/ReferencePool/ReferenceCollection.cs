using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using UnityEngine;

namespace SimpleGameFramework.ReferencePool
{
    /// 引用集合，收集同一类型的所有引用，会被引用池直接管理
    public class ReferenceCollection
    {
        #region Private

        private Queue<IReference> m_References;

        #endregion

        #region 构造方法

        public ReferenceCollection()
        {
            m_References = new Queue<IReference>();
        }

        #endregion

        #region Public 接口方法

        /// 获取指定类型的引用
        public T Acquire<T>() where T : class, IReference, new()
        {
#if UNITY_EDITOR
            var trans = SGFEntry.Instance.transform.Find("ReferenceManager");
            var collection = trans.Find(typeof(T).FullName);
#endif
            lock (m_References)
            {
                if (m_References.Count > 0)
                {
#if UNITY_EDITOR
                    if (collection != null)
                    {
                        // 找到一个active的孩子，设置为fals
                        for (int i = 0; i < collection.childCount; i++)
                        {
                            if (collection.GetChild(i).gameObject.activeSelf)
                            {
                                collection.GetChild(i).gameObject.SetActive(false);
                                break;
                            }
                        }
                    }
#endif
                    return m_References.Dequeue() as T;
                }
            }
#if UNITY_EDITOR
            if (collection != null)
            {
                GameObject go = new GameObject();
                go.name = "reference";
                go.transform.SetParent(collection);
                go.gameObject.SetActive(false);
            }
#endif
            return new T();
        }
        
        /// 释放引用
        public void Release<T>(T reference) where T : class, IReference
        {
            reference.Clear();
            lock (m_References)
            {
#if UNITY_EDITOR
                var trans = SGFEntry.Instance.transform.Find("ReferenceManager");
                var collection = trans.Find(typeof(T).FullName);
                if (collection != null)
                {
                    for (int i = 0; i < collection.childCount; i++)
                    {
                        if (!collection.GetChild(i).gameObject.activeSelf)
                        {
                            collection.GetChild(i).gameObject.SetActive(true);
                            break;
                        }
                    }
                }
#endif
                m_References.Enqueue(reference);
            }
        }

        /// 删除所有引用
        public void RemoveAll()
        {
            lock (m_References)
            {
                m_References.Clear();
            }
        }

        #endregion
    }
}

