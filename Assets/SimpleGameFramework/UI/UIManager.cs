using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleGameFramework.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : ManagerBase
{
    #region Field

    // 分别缓存已经加载的三种UI，被加载过不代表正在显示
    private LinkedList<UIBase> allFixedUI;
    private LinkedList<UIBase> allNormalUI;
    private LinkedList<UIBase> allPopUpUI;

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
        allFixedUI = new LinkedList<UIBase>();
        allNormalUI = new LinkedList<UIBase>();
        allPopUpUI = new LinkedList<UIBase>();

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
    }

    public override void Update(float time)
    {
        
    }

    public override void ShutDown()
    {
        
    }

    #endregion

    #region 接口方法

    public void OpenUI(UIStruct data)
    {
        // 如果这个UI还没被加载，那需要先加载
        if (!uiLoaded.TryGetValue(data.name,out var ui)) 
        {
            LoadUI(data,out ui);
        }
        // 显示UI
        ShowUI(ui);
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
        
        // 挂载脚本
        ui = prefab.AddComponent(script) as UIBase;
        uiLoaded.Add(data.name, ui);

        // 根据UI类型，存放到不同的节点中
        switch (ui.type)
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
        switch (ui.type)
        {
            case UIType.Fixed:
                AddToList(ui,allFixedUI);
                break;
            case UIType.Normal:
                AddToList(ui,allNormalUI);
                break;
            case UIType.PopUp:
                AddToList(ui,allPopUpUI);
                break;
        }
    }

    /// <summary>
    /// 将UI加入对应链表，并真正的控制当前UI的显示隐藏逻辑
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="list"></param>
    private void AddToList(UIBase ui, LinkedList<UIBase> list)
    {
        // 打开一个新的UI，当前UI必然被冻结，但未必会隐藏，打开Normal会隐藏Normal但不会隐藏Fixed
        if (currentUI != null) 
        {
            // 冻结当前的界面
            currentUI.Freeze();
            // 如果打开的不是Pop界面，且打开的界面和当前界面是同类型的，那才需要关闭当前的界面
            if (ui.type != UIType.PopUp && ui.type == currentUI.type)  
            {
                currentUI.Close();
                currentUI.gameObject.SetActive(false);
            }
        }
        ui.Show();
        list.AddLast(ui);
        currentUI = ui;
    }

    #endregion
}
