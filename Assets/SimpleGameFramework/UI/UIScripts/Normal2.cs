using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal2 : UIBase
{
    public override void Load()
    {
        Debug.Log("加载 Normal2");
    }

    public override void UnLoad()
    {
        Debug.Log("卸载 Normal2");
    }

    public override void Show()
    {
        Debug.Log("显示 Normal2");
    }

    public override void Close()
    {
        Debug.Log("关闭 Normal2");
    }

    public override void Freeze()
    {
        Debug.Log("冻结 Normal2");
    }

    public override void UnFreeze()
    {
        Debug.Log("解冻 Normal2");
    }
}
