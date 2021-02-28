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
        Debug.Log("清理");
        name = String.Empty;
    }

    public EventTestArgs()
    {
        Debug.Log("构造");
    }
}

public class test : MonoBehaviour
{
    private EventManager m_EventManager;
    private ReferenceManager m_ReferenceManager;
    void Start()
    {
        m_EventManager = SGFEntry.Instance.GetManager<EventManager>();
        m_ReferenceManager = SGFEntry.Instance.GetManager<ReferenceManager>();
    }
    
    private IEnumerator TestCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            yield return null;
        }
        yield break;
        Debug.Log("break");
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
