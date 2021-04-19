using System;
using SimpleGameFramework.Core;
using UnityEngine;

namespace SimpleGameFramework.Event
{
    public class EventManager : ManagerBase
    {
        #region Private

        /// 事件池，维护一个事件池，其实事件管理器就是对事件池的代理
        private EventPool<GlobalEventArgs> m_EventPool;

        #endregion

        #region 构造方法

        public EventManager()
        {
            m_EventPool = new EventPool<GlobalEventArgs>();
        }

        #endregion
        
        #region Override

        public override int Priority
        {
            get { return ManagerPriority.EventManager.GetHashCode(); }
        }

        public override void Init()
        {
            
        }

        public override void Update(float time)
        {
            m_EventPool.Update(time);
        }

        public override void ShutDown()
        {
            
        }

        #endregion

        #region Public 接口方法

 
        /// 订阅事件
        public void Subscribe(SGFEvents id, EventHandler<GlobalEventArgs> handler)
        {
            #if UNITY_EDITOR
            var trans = SGFEntry.Instance.transform.Find("EventManager");
            var name = id.ToString();
            var temp = trans.Find(name);
            if (temp == null)   
            {
                GameObject go = new GameObject();
                go.name = name;
                go.transform.SetParent(trans);
                
                GameObject go2 = new GameObject();
                go2.name = $"{handler.Target}：{handler.Method.Name}";
                go2.transform.SetParent(go.transform);
            }
            else
            {
                GameObject go2 = new GameObject();
                go2.name = $"{handler.Target}：{handler.Method.Name}";
                go2.transform.SetParent(temp.transform);
            }
            #endif
            m_EventPool.Subscribe(id.GetHashCode(), handler);
        }
 
        /// 取消订阅事件
        public void Unsubscribe(SGFEvents id, EventHandler<GlobalEventArgs> handler)
        {
            m_EventPool.Unsubscribe(id.GetHashCode(), handler);
#if UNITY_EDITOR
            var trans = SGFEntry.Instance.transform.Find("EventManager");
            var group = trans.Find(id.ToString());
            var child = group.Find($"{handler.Target}：{handler.Method.Name}");
            if (child != null) 
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            if (group.childCount <= 0) 
            {
                GameObject.DestroyImmediate(group.gameObject);
            }
#endif
        }
        
        /// 抛出事件（线程安全）
        public void Fire(object sender, GlobalEventArgs e)
        {
            m_EventPool.Fire(sender, e);
        }
        
        /// 抛出事件（线程不安全）
        public void FireNow(object sender, GlobalEventArgs e)
        {
            m_EventPool.FireNow(sender, e);
        }
        #endregion
    }
}