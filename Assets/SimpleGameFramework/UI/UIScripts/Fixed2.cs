using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixed2 : UIBase
{
    public override void Load()
    {
        UIType = UIType.Fixed;
        Debug.Log("加载 Fixed2");
    }

    public override void UnLoad()
    {
        Debug.Log("卸载 Fixed2");
    }

    public override void Show()
    {
        Debug.Log("显示 Fixed2");
    }

    public override void Close()
    {
        Debug.Log("关闭 Fixed2");
    }

    public override void Freeze()
    {
        Debug.Log("冻结 Fixed2");
    }

    public override void UnFreeze()
    {
        Debug.Log("解冻 Fixed2");
    }
}
