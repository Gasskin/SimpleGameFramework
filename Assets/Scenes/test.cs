using SimpleGameFramework.Core;
using SimpleGameFramework.Procedure;
using SimpleGameFramework.ReferencePool;
using SimpleGameFramework.UI;
using UnityEngine;


public class Procedure_Loading : ProcedureBase
{
    private float time = 0;
    
    public override void OnUpdate(Fsm<ProcedureManager> fsm, float time)
    {
        this.time += time;
        if (this.time > 3)
        {
            ChangeState<Procedure_Gaming>(fsm);
        }
    }
}

public class Procedure_Gaming : ProcedureBase
{
    public override void OnUpdate(Fsm<ProcedureManager> fsm, float time)
    {
        Debug.Log("游戏中...");
        if (Input.GetMouseButtonDown(0))
        {
            ChangeState<Procedure_Pause>(fsm);
        }
    }
}

public class Procedure_Pause : ProcedureBase
{
    public override void OnUpdate(Fsm<ProcedureManager> fsm, float time)
    {
        Debug.Log("暂停中...");
        if (Input.GetMouseButtonDown(0)) 
        {
            ChangeState<Procedure_Gaming>(fsm);
        }
    }
}


public class test : MonoBehaviour
{
    void Start()
    {
        var procedureManager = SGFEntry.Instance.GetManager<ProcedureManager>();
        
        Procedure_Loading loading = new Procedure_Loading();
        procedureManager.AddProcedure(loading);
        procedureManager.SetEntranceProcedure(loading);

        procedureManager.AddProcedure(new Procedure_Gaming());
        procedureManager.AddProcedure(new Procedure_Pause());
        
        procedureManager.CreateProceduresFsm();
    }
}


