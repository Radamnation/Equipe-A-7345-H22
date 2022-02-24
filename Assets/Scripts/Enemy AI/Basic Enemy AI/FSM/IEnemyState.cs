using UnityEngine;

public interface IEnemyState
{
    // SECTION - Method - State Specific =================================================================== 
    public void OnMovement(BasicEnemyContext context);

    public void OnAttackMain(BasicEnemyContext context);

    public void OnManageToken(BasicEnemyContext context);


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context);
    public void OnStateUpdate(BasicEnemyContext context);
    public IEnemyState OnStateExit(BasicEnemyContext context);
}
