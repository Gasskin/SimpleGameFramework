using System;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.Event;
using SimpleGameFramework.ReferencePool;
using UnityEngine;

namespace SimpleGameFramework.UI
{
    public class UIManager : ManagerBase
    {
        #region Field

        // 分别缓存已经加载的三种UI，被加载过不代表正在显示
        private Stack<UIBase> allFixedUI;
        private Stack<UIBase> allNormalUI;
        private Stack<UIBase> allPopUpUI;

        /// 这里记录了所有已经被加载的UI，其实就是上面三个之和
        private Dictionary<string, UIBase> uiLoaded;
    
        /// 当前打开的UI
        private UIBase currentUI;
    
        /// 根节点，即UIRoot 
        private Transform uiRoot;
        /// fixed界面的节点 
        private Transform fixedRoot;
        /// normal界面的节点
        private Transform normalRoot;
        /// popUp界面的节点
        private Transform popUpRoot;

        /// 事件管理器
        private EventManager eventManager;

        /// 引用池
        private ReferenceManager referenceManager;
    
        #endregion
    
        #region Proprity

        public override int Priority
        {
            get { return ManagerPriority.UIManager.GetHashCode(); }
        }

        #endregion

        #region 管理器生命周期

        public override void Init()
        {
            // 初始化
            allFixedUI = new Stack<UIBase>();
            allNormalUI = new Stack<UIBase>();
            allPopUpUI = new Stack<UIBase>();

            uiLoaded = new Dictionary<string, UIBase>();

            currentUI = null;
        
            // 保存各个节点的信息
            if (null == uiRoot)
            {
                // 加载UIRoot Prefab
                GameObject go = Resources.Load<GameObject>("UI/UIRoot");
                var prefab = GameObject.Instantiate(go);

                // 初始化节点信息
                var uiNode = SGFEntry.Instance.transform.Find("UIManager");
                prefab.transform.SetParent(uiNode);
                uiRoot = prefab.transform;
                fixedRoot = uiRoot.Find("Fixed");
                normalRoot = uiRoot.Find("Normal");
                popUpRoot = uiRoot.Find("PopUp");
                if (uiRoot == null || fixedRoot == null || normalRoot == null || popUpRoot == null) 
                {
                    throw new Exception("UI节点初始化失败");
                }
            }
        
            // 初始化管理器
            eventManager = SGFEntry.Instance.GetManager<EventManager>();
            referenceManager = SGFEntry.Instance.GetManager<ReferenceManager>();
        }

        public override void Update(float time)
        {
            currentUI.OnUpdate(time);
        }

        public override void ShutDown()
        {
        
        }

        #endregion

        #region 接口方法

        /// <summary>
        /// 打开一个UI
        /// </summary>
        public void Open(UIStruct data, UIOpenEventArgs uiOpenEventArgs)
        {
            // 如果这个UI还没被加载，那需要先加载
            if (!uiLoaded.TryGetValue(data.name,out var ui)) 
            {
                LoadUI(data,out ui);
            }
            // 显示UI
            ShowUI(ui);
            // 发送打开UI事件
            uiOpenEventArgs.uiName = ui.uiName;
            eventManager.FireNow(this,uiOpenEventArgs);
        }

        /// <summary>
        /// 关闭当前UI，注意此方法只关闭当前打开的最顶层UI
        /// </summary>
        public void CloseCurrent()
        {
            if (currentUI == null) 
            {
                Debug.LogError("没有可以关闭的界面");
                return;
            }

            switch (currentUI.UIType)
            {
                case UIType.Fixed:
                    PopFromStack(allFixedUI);
                    break;
                case UIType.Normal:
                    PopFromStack(allNormalUI);
                    break;
                case UIType.PopUp:
                    PopFromStack(allPopUpUI);
                    break;
            }
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 加载UI
        /// </summary>
        private void LoadUI(UIStruct data, out UIBase ui)
        {
            GameObject prefab = null;

            if (String.IsNullOrEmpty(data.path))
            {
                throw new Exception("非法的UI路径");
            }

            // 加载预制体
            prefab = Resources.Load<GameObject>(data.path);
            prefab = GameObject.Instantiate(prefab);

            if (prefab == null)
            {
                throw new Exception($"加载UI Prefab失败，请检查是否存在预制体：{data.path}");
            }

            Type script = Type.GetType(data.name);

            if (script == null)
            {
                throw new Exception($"加载脚本失败，请检查是否存在脚本：{data.name}");
            }
        
            // 挂载脚本，并指定UI名称
            ui = prefab.AddComponent(script) as UIBase;
            ui.uiName = data.name;
            
            uiLoaded.Add(data.name, ui);

            // 根据UI类型，存放到不同的节点中
            switch (ui.UIType)
            {
                case UIType.Fixed:
                    prefab.transform.SetParent(fixedRoot,false);
                    break;
                case UIType.Normal:
                    prefab.transform.SetParent(normalRoot,false);
                    break;
                case UIType.PopUp:
                    prefab.transform.SetParent(popUpRoot,false);
                    break;
            }
        }

        /// <summary>
        /// 显示UI 
        /// </summary>
        private void ShowUI(UIBase ui)
        {
            switch (ui.UIType)
            {
                case UIType.Fixed:
                    PushToStack(ui,allFixedUI);
                    break;
                case UIType.Normal:
                    PushToStack(ui,allNormalUI);
                    break;
                case UIType.PopUp:
                    PushToStack(ui,allPopUpUI);
                    break;
            }
        }

        /// <summary>
        /// 将UI加入对应链表，并真正的控制当前UI的显示隐藏逻辑
        /// </summary>
        private void PushToStack(UIBase ui, Stack<UIBase> stack)
        {
            // 打开一个新的UI，当前UI必然被冻结，但未必会隐藏，打开Normal会隐藏Normal但不会隐藏Fixed
            if (currentUI != null) 
            {
                // 冻结当前的界面
                currentUI.Freeze();
                // 如果打开的不是Pop界面，且打开的界面和当前界面是同类型的，那才需要关闭当前的界面
                if (ui.UIType != UIType.PopUp && ui.UIType == currentUI.UIType)  
                {
                    currentUI.Close();
                    currentUI.gameObject.SetActive(false);
                }
            }
            if (!ui.gameObject.activeSelf)
            {
                ui.gameObject.SetActive(true);
            }
            ui.Show();
            stack.Push(ui);
            currentUI = ui;
        }

        /// <summary>
        /// 将UI从对应链表中移除，并真正的控制当前UI的关闭 
        /// </summary>
        private void PopFromStack(Stack<UIBase> stack)
        {
            // 首先关闭并冻结当前的UI
            currentUI.Close();
            currentUI.Freeze();
            currentUI.gameObject.SetActive(false);
            // 栈顶UI出栈，其实就是currentUI
            stack.Pop();
            // 然后尝试获取当前UI栈中的上一个UI
            if (stack.Count >= 1)
            {
                currentUI = stack.Peek();
                // 注意，如果当前栈是Pop栈，那么我们关闭顶层的弹窗是不需要重新激活上一个弹窗的，因为Pop类型UI之间不会相互关闭，我们只需要解冻就好
                if (stack != allPopUpUI) 
                {
                    currentUI.Show();
                    currentUI.gameObject.SetActive(true);
                }
                currentUI.UnFreeze();
            }
            // 如果当前栈里没有UI了，那就获取上一个栈中的UI
            else
            {
                // 如果当前栈是Pop，那么currentUI就是Noraml栈中的最后一个元素，其余同理
                // 另外，因为不同类型的UI之间也不会相互关闭，所以我们也只需要解冻就好
                if (stack == allPopUpUI)
                {
                    currentUI = allNormalUI.Peek();
                    currentUI.UnFreeze();
                }
                else if (stack == allNormalUI)
                {
                    currentUI = allFixedUI.Peek();
                    currentUI.UnFreeze();
                }
                else
                {
                    currentUI = null;
                }
            }
        }
    
        #endregion
    }
}
