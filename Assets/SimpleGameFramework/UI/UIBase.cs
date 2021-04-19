using SimpleGameFramework.Core;
using SimpleGameFramework.Event;
using UnityEngine;

namespace SimpleGameFramework.UI
{
    public abstract class UIBase : MonoBehaviour
    {
        #region Field

        /// 窗口类型，默认是Normal
        private UIType type = UIType.Normal;

        /// 事件管理器
        private EventManager eventManager;

        /// UI的名字，不可重复
        public string uiName;
    
        #endregion

        #region Property

        /// 窗口类型
        public UIType UIType
        {
            get { return type; }
            set { type = value; }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            eventManager = SGFEntry.Instance.GetManager<EventManager>();
            eventManager.Subscribe(SGFEvents.OpenUI,AfterShow);
            eventManager.Subscribe(SGFEvents.CloseUI,AfterClose);
            eventManager.Subscribe(SGFEvents.RefreshUI,Refresh);
            Load();
        }

        private void OnDestroy()
        {
            eventManager.Unsubscribe(SGFEvents.OpenUI,AfterShow);
            eventManager.Unsubscribe(SGFEvents.CloseUI,AfterClose);
            eventManager.Unsubscribe(SGFEvents.RefreshUI,Refresh);
            UnLoad();
        }

        #endregion

        #region 生命周期

        /// <summary>
        /// 加载UI，类似于Awake()，多用于初始化
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// 卸载UI，类似于OnDestroy()，多用于卸载
        /// </summary>
        public abstract void UnLoad();

        /// <summary>
        /// 会在UI管理器中被统一调用，用于更新
        /// </summary>
        public virtual void OnUpdate(float deltaTime)
        {

        }

        /// <summary>
        /// 打开UI
        /// </summary>
        public virtual void Show()
        {

        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        public virtual void Close()
        {

        }

        /// <summary>
        /// 冻结UI
        /// </summary>
        public virtual void Freeze()
        {

        }

        /// <summary>
        /// 解冻UI
        /// </summary>
        public virtual void UnFreeze()
        {

        }

        #endregion

        #region 工具方法

        private void AfterShow(object o, GlobalEventArgs e)
        {
            var temp = e as UIOpenEventArgs;
            if (!temp.uiName.Equals(uiName))
            {
                return;
            }
            DoAfterShow(o,temp);
        }

        private void AfterClose(object o, GlobalEventArgs e)
        {
            var temp = e as UIOpenEventArgs;
            if (!temp.uiName.Equals(uiName))
            {
                return;
            }
            DoAfterClose(o,temp);
        }
    
        private void Refresh(object o, GlobalEventArgs e)
        {
            var temp = e as UIOpenEventArgs;
            if (!temp.uiName.Equals(uiName))
            {
                return;
            }
            DoRefresh(o,temp);
        }

        #endregion

        #region 事件

        /// <summary>
        /// 会在这个UI显示后调用 
        /// </summary>
        public virtual void DoAfterShow(object o, UIOpenEventArgs e)
        {
        
        }

        /// <summary>
        /// 会在这个UI关闭后调用
        /// </summary>
        public virtual void DoAfterClose(object o, UIOpenEventArgs e)
        {
        
        }
    
        /// <summary>
        /// 会在这个UI被刷新时调用
        /// </summary>
        public virtual void DoRefresh(object o, UIOpenEventArgs e)
        {
        
        }

        #endregion


        
    }
}
