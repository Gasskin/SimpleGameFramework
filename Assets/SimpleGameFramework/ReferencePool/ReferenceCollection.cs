using System.Collections;
using System.Collections.Generic;

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
            lock (m_References)
            {
                if (m_References.Count > 0)
                {
                    return m_References.Dequeue() as T;
                }
            }
            return new T();
        }
        
        /// 释放引用
        public void Release<T>(T reference) where T : class, IReference
        {
            reference.Clear();
            lock (m_References)
            {
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