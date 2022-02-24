using UnityEngine;

public class EnemyStateRoaming : IEnemyState
{
    // SECTION - Method - State Specific ===================================================================
    public void OnMovement(BasicEnemyContext context) 
    {
        context.OnDefaultSetMoveAnim();
        context.OnDefaultMoveBehaviour();

        if (!context.IsInAnimationState(BasicEnemy_AnimationStates.ROAMINGATTACK) &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.AGGRESSIVEATTACK) &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) &&
            !context.IsMainWeaponReloading())
        {       
            if (context.R_MoveBehaviour != null)
                context.R_MoveBehaviour.Execute();
        }
        else context.SetTargetAsPlayer(); // Prevents target being null     
    }

    public void OnAttackMain(BasicEnemyContext context)
    {
        if (context.R_AtkBehaviour != null)
        {
            if (context.MyAIPath.reachedEndOfPath &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ROAMINGATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.AGGRESSIVEATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) &&
                 context.IsMainWeaponReloading())
            {
                context.ReloadMainWeapon();

                // Check if invoke now or wait for animation event
                if (!context.RAnimExecuteAtk)
                {
                    if (context.R_AtkBehaviour.IsExecutionValid())
                    {
                        context.OnDefaultAttackBehaviour();
                        context.R_AtkBehaviour.Execute();
                    }
                }
                else
                    context.SetAnimTrigger(BasicEnemy_AnimTriggers.ROAMINGATTACK);
            }
        }
    }

    public void OnManageToken(BasicEnemyContext context)
    {
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context)
    {
        context.SetEndReachedDistance(context.RWeaponManager.MainWeapon.Range);
    }

    public void OnStateUpdate(BasicEnemyContext context)
    {
        OnMovement(context);
        OnAttackMain(context);
        OnManageToken(context);
    }

    public IEnemyState OnStateExit(BasicEnemyContext context)
    {
        // Dead
        if (context.MyLivingEntity.IsDead)
            return new EnemyStateDead();

        return this;
    }
}
