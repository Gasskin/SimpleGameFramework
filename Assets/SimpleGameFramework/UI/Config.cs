using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI的类型
/// </summary>
public enum UIType
{
    /// 固定类型，一般作为背景
    Fixed,
    /// 普通窗口，一般窗口都用这个
    Normal,
    /// 弹窗，可以在某些窗口上弹出的窗口
    PopUp
}


/// <summary>
/// UI的注册
/// </summary>
public enum UIRegister
{
    Fixed1,
    Fixed2,
    Normal1,
    Normal2,
    Pop1,
    Pop2,
}