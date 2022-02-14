using UnityEngine;

public class EnemyStateAgressive : IEnemyState
{
    // SECTION - Method - State Specific ===================================================================
    public void OnAttack(EnemyContext context)
    {
    }

    public void OnAttackMain(EnemyContext context)
    {
    }

    public void OnAttackSecondary(EnemyContext context)
    {
    }

    public void OnManageToken(EnemyContext context)
    {
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(EnemyContext context)
    {
    }

    public void OnStateUpdate(EnemyContext context)
    {
    }

    public IEnemyState OnStateExit(EnemyContext context)
    {
        return this;
    }
}
