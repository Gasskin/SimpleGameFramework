using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    #region Field

    /// 窗口类型，默认是Normal
    private UIType type = UIType.Normal;

    #endregion

    #region Property

    /// 窗口类型
    public UIType UIType
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

    #endregion
    
    #region MonoBehaviour

    private void Awake()
    {
        Load();
    }

    private void OnDestroy()
    {
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
}
