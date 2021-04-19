using SimpleGameFramework.Core;
using SimpleGameFramework.Event;
using SimpleGameFramework.UI;
using UnityEngine;

public class Fixed2 : UIBase
{
    private EventManager eventManager;
    public override void Load()
    {
        UIType = UIType.Fixed;
        
        eventManager = SGFEntry.Instance.GetManager<EventManager>();
        
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

    public override void DoAfterShow(object o, UIOpenEventArgs e)
    {
        Debug.Log(e.data);
    }
}
