using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameFramework.Core
{
    /// <summary>
    /// 框架入口，管理所有的模块
    /// </summary>
    public class SGFEntry : ScriptSingleton<SGFEntry>
    {
        #region Private
        /// 维护了所有的管理器，并按照管理器的优先级由大到小排序
        private LinkedList<ManagerBase> m_Managers = new LinkedList<ManagerBase>();
        #endregion

        #region MonoBehaviour

        /// 依次更新所有的管理器
        private void Update()
        {
            foreach (var manager in m_Managers)
            {
                manager.Update(Time.deltaTime);
            }
        }

        /// 倒序销毁所有的管理器
        private void OnDestroy()
        {
            for (var manager = m_Managers.Last; manager != null; manager = manager.Previous)
            {
                manager.Value.ShutDown();
            }
            m_Managers.Clear();
        }

        #endregion

        #region Public 接口方法

        
        /// 从管理器链表中获取指定的管理器，如果没有，那会创建一个对应的管理器，并加入管理器链表 
        public TManager GetManager<TManager>() where TManager : ManagerBase
        {
            Type managerType = typeof(TManager);
            // 检查是否存在对应管理器
            foreach (var manager in m_Managers)
            {
                if (manager.GetType() == managerType)
                {
                    return manager as TManager;
                }
            }
            // 不存在就创建
            return CreateManager(managerType) as TManager;
        }

        #endregion

        #region Private 工具方法
        
        /// 创建一个管理器 
        private ManagerBase CreateManager(Type managerType)
        {
            ManagerBase manager = Activator.CreateInstance(managerType) as ManagerBase;

            if (manager == null)
            {
                throw new Exception("创建管理器失败...");
            }
            
            // 根据模块优先级决定它在链表里的位置
            LinkedListNode<ManagerBase> current = m_Managers.First;
            while (current != null)
            {
 
                if (manager.Priority > current.Value.Priority)
                {
                    break;
                }
 
                current = current.Next;
            }
            // 如果存在current，那么会在他前面插入manager
            if (current != null)
            {
 
                m_Managers.AddBefore(current, manager);
            }
            // 如果不存在current，说明所有节点的优先级都比当前manager高，那就插入到末尾
            else
            {
 
                m_Managers.AddLast(manager);
            }
 
            // 初始化管理器
            manager.Init();
            return manager;
        }

        #endregion
    }
}