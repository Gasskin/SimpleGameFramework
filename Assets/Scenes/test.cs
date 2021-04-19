using SimpleGameFramework.Core;
using SimpleGameFramework.ReferencePool;
using SimpleGameFramework.UI;
using UnityEngine;


public class test : MonoBehaviour
{
    private UIManager uiManager;
    private ReferenceManager referenceManager;
    void Start()
    {
        uiManager = SGFEntry.Instance.GetManager<UIManager>();
        referenceManager = SGFEntry.Instance.GetManager<ReferenceManager>();
        var eventArgs = referenceManager.Acquire<UIOpenEventArgs>();
        eventArgs.data = "===Fixed1===";
        uiManager.Open(UIs.Fixed1,eventArgs);
        //referenceManager.Release(eventArgs);
    }
}


