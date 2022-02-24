using UnityEngine;

public class EnemyStateAgressive : IEnemyState
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
            if (context.A_MoveBehaviour != null)
                context.A_MoveBehaviour.Execute();
        }
        else context.SetTargetAsPlayer(); // Prevents target being null
    }

    public void OnAttackMain(BasicEnemyContext context)
    {
        if (context.A_AtkBehaviour != null)
        {
            if (context.MyAIPath.reachedEndOfPath &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ROAMINGATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.AGGRESSIVEATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) &&
                 context.IsMainWeaponReloading())
            {
                context.ReloadMainWeapon();

                // Check if invoke now or wait for animation event
                if (!context.AAnimExecuteAtk)
                {
                    if (context.A_AtkBehaviour.IsExecutionValid())
                    {
                        context.OnDefaultAttackBehaviour();
                        context.A_AtkBehaviour.Execute();
                    }
                }
                else
                    context.SetAnimTrigger(BasicEnemy_AnimTriggers.AGGRESSIVEATTACK);
            }
        }
    }

    public void OnManageToken(BasicEnemyContext context)
    {
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context)
    {
        context.SetEndReachedDistance(context.AWeaponManager.MainWeapon.Range);
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
