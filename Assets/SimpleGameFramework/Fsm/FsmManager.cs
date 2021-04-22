using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.Fsm;

public class FsmManager : ManagerBase
{
    /// <summary>
    /// 所有状态机
    /// </summary>
    private Dictionary<string, IFsm> m_Fsms;
    private List<IFsm> m_TempFsms;

    public override int Priority
    {
        get
        {
            return ManagerPriority.FsmManager.GetHashCode();
        }
    }

    public override void Init()
    {
        m_Fsms = new Dictionary<string, IFsm>();
        m_TempFsms = new List<IFsm>();
    }

    public override void Update(float time)
    {
        m_TempFsms.Clear();
        if (m_Fsms.Count <= 0)
        {
            return;
        }
        
        // 这里用一个额外链表的作用是，字典类型的数据结构，foreach时无法增删
        foreach (KeyValuePair<string, IFsm> fsm in m_Fsms)
        {
            m_TempFsms.Add(fsm.Value);
        }
        
        foreach (IFsm fsm in m_TempFsms)
        {
            if (fsm.IsDestroyed)
            {
                continue;
            }
            //轮询状态机
            fsm.Update(time);
        }
    }

    
    /// <summary>
    /// 关闭并清理状态机管理器
    /// </summary>
    public override void ShutDown()
    {
        foreach (KeyValuePair<string, IFsm> fsm in m_Fsms)
        {
            fsm.Value.Shutdown();
        }
 
 
        m_Fsms.Clear();
        m_TempFsms.Clear();
    }

}