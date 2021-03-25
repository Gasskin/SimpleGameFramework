using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixed1 : UIBase
{
    public override void Load()
    {
        UIType = UIType.Fixed;
        Debug.Log("加载 Fixed1");
    }

    public override void UnLoad()
    {
        Debug.Log("卸载 Fixed1");
    }

    public override void Show()
    {
        Debug.Log("显示 Fixed1");
    }

    public override void Close()
    {
        Debug.Log("关闭 Fixed1");
    }

    public override void Freeze()
    {
        Debug.Log("冻结 Fixed1");
    }

    public override void UnFreeze()
    {
        Debug.Log("解冻 Fixed1");
    }
}
