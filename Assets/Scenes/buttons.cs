using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using UnityEngine;

public class buttons : MonoBehaviour
{
    
    private UIManager uiManager;
    void Start()
    {
        uiManager = SGFEntry.Instance.GetManager<UIManager>();
    }
    
    public void OpenFixed2()
    {
        uiManager.Open(UIs.Fixed2);
    }

    public void OpenNormal1()
    {
        uiManager.Open(UIs.Normal1);
    }

    public void OpenNormal2()
    {
        uiManager.Open(UIs.Normal2);
    }

    public void OpenPop1()
    {
        uiManager.Open(UIs.Pop1);
    }

    public void OpenPop2()
    {
        uiManager.Open(UIs.Pop2);
    }

    public void Close()
    {
        uiManager.CloseCurrent();
    }
}
