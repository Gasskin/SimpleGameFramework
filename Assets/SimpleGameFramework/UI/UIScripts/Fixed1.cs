using SimpleGameFramework.Core;
using SimpleGameFramework.ReferencePool;
using SimpleGameFramework.UI;
using SimpleGameFramework.UI.Generated;
using UnityEngine;

public class Fixed1 : UIBase
{
    private UIManager uiManager;
    private ReferenceManager referenceManager;
    public override void Load()
    {
        UIType = UIType.Fixed;
        
        uiManager = SGFEntry.Instance.GetManager<UIManager>();
        referenceManager = SGFEntry.Instance.GetManager<ReferenceManager>();
        
        var eventArgs1 = referenceManager.Acquire<UIOpenEventArgs>();
        var eventArgs2 = referenceManager.Acquire<UIOpenEventArgs>();
        //referenceManager.Release(eventArgs1);
        //referenceManager.Release(eventArgs2);
        
        transform.RegisterButton("Button",(() =>
        {
            var eventArgs = referenceManager.Acquire<UIOpenEventArgs>();
            eventArgs.data = "===Fixed2===";
            uiManager.Open(UIs.Fixed2,eventArgs);
            referenceManager.Release(eventArgs);
        }));
        
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

    public override void DoAfterShow(object o, UIOpenEventArgs e)
    {
        Debug.Log(e.data);
    }
}
