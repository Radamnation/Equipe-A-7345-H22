using UnityEngine;

public class EnemyStateDead : IEnemyState
{
    // SECTION - Method - State Specific ===================================================================
    public void OnMovement(BasicEnemyContext context)
    {
    }

    public void OnAttack(BasicEnemyContext context)
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
    }

    public void OnStateUpdate(BasicEnemyContext context)
    {
    }

    public IEnemyState OnStateExit(BasicEnemyContext context)
    {
        return this;
    }
}
