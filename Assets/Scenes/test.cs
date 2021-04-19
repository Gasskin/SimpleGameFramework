using SimpleGameFramework.Core;
using SimpleGameFramework.ReferencePool;
using SimpleGameFramework.UI;
using UnityEngine;


public class test : MonoBehaviour
{
    private ReferenceManager referenceManager;
    void Start()
    {
        referenceManager = SGFEntry.Instance.GetManager<ReferenceManager>();
        var tempRef = referenceManager.Acquire<UIOpenEventArgs>();
        tempRef.data = "测试：打开UI的事件！";
        SGFEntry.Instance.GetManager<UIManager>().Open(UIs.Fixed1, tempRef);
    }
}


