using System;
using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : ManagerBase
{
    #region Field

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
            if (fixedRoot == null || normalRoot == null || popUpRoot == null)
            {
                throw new Exception("==== UI节点初始化失败 ===");
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

    public void OpenUI()
    {
        GameObject go = Resources.Load<GameObject>(UIPath.TESTUI);
        var prefab = GameObject.Instantiate(go);
        prefab.transform.SetParent(normalRoot,false);
    }

    #endregion
}
