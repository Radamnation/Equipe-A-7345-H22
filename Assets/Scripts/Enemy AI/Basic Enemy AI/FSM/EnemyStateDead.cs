using UnityEngine;

public class EnemyStateDead : IEnemyState
{
    // SECTION - Method - State Specific ===================================================================
    public void WithoutTokenBehaviour(BasicEnemyContext context)
    {
    }

    public void WithTokenBehaviour(BasicEnemyContext context)
    {
    }

    public void OnManageToken(BasicEnemyContext context)
    {
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context)
    {
        // Disable Movement
        context.SetSpeed(0.0f);

        // Disable passive behaviours
        context.transform.GetChild(4).gameObject.SetActive(false);

        // Disable State Machine
        context.enabled = false;

        // Replace Token uppon death
        AIManager.instance.MyTokenHandlerSO.ReturnToken(context.HasToken);
    }

    public void OnStateUpdate(BasicEnemyContext context)
    {
    }

    public IEnemyState OnStateExit(BasicEnemyContext context)
    {
        return this;
    }
}
