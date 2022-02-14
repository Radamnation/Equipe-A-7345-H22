using UnityEngine;

public interface IEnemyState
{
    // SECTION - Method - State Specific =================================================================== 
    public void OnAttackMain(EnemyContext context); // Change for OnAttack only ??
    public void OnAttackSecondary(EnemyContext context); // Change for OnAttack only ??

    public void OnAttack(EnemyContext context);


    public void OnManageToken(EnemyContext context);


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(EnemyContext context);
    public void OnStateUpdate(EnemyContext context);
    public IEnemyState OnStateExit(EnemyContext context);
}
