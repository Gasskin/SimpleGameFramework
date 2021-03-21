using System;
using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.Event;
using SimpleGameFramework.ReferencePool;
using UnityEngine;
using UnityEngine.Profiling;


public class test : MonoBehaviour
{
    private UIManager uiManager;
    void Start()
    {
        uiManager = SGFEntry.Instance.GetManager<UIManager>();
        uiManager.Open(UIs.Fixed1);
    }

    
}
