using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.UI;
using UnityEngine;

public class Pop1 : UIBase
{
    public override void Load()
    {
        UIType = UIType.PopUp;
        Debug.Log("加载 Pop1");
    }

    public override void UnLoad()
    {
        Debug.Log("卸载 Pop1");
    }

    public override void Show()
    {
        Debug.Log("显示 Pop1");
    }

    public override void Close()
    {
        Debug.Log("关闭 Pop1");
    }

    public override void Freeze()
    {
        Debug.Log("冻结 Pop1");
    }

    public override void UnFreeze()
    {
        Debug.Log("解冻 Pop1");
    }
}
