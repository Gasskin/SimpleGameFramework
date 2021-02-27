using System;
using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.Event;
using SimpleGameFramework.ReferencePool;
using UnityEngine;
using UnityEngine.Profiling;

public class EventTestArgs : GlobalEventArgs
{
    public string name = "test test test";
    public override int Id
    {
        get { return 1; }
    }
    public override void Clear()
    {
        name = String.Empty;
    }
}

public class test : MonoBehaviour
{
    private EventManager m_EventManager;
    void Start()
    {
        m_EventManager =  SGFEntry.Instance.GetManager<EventManager>();
        m_EventManager.Subscribe(SGFEvents.TEST, EventTestMethod);
    }

    private void Update()
    {
        Profiler.BeginSample("==Test==");

        // EventTestArgs e = ReferencePool.Acquire<EventTestArgs>();
        EventTestArgs e = new EventTestArgs();
        e.name = "test test test";
        m_EventManager.FireNow(this,e);

        Profiler.EndSample();
        
    }

    private void EventTestMethod(object sender, GlobalEventArgs e)
    {
        EventTestArgs args = e as EventTestArgs;
        Debug.Log("method..");
        Debug.Log("obj:"+sender);
        Debug.Log("name:"+args.name);
        
        // ReferencePool.Release(args);
    }
}
