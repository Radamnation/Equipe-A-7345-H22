using UnityEngine;

public class BasicEnemyState_Two : IEnemyState
{
    // SECTION - Method - State Specific ===================================================================
    public void WithoutTokenBehaviour(BasicEnemyContext context)
    {
        context.OnDefaultSetMoveAnim();
        context.OnDefaultMoveBehaviour();

        if (!context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_ONE_ATTACK) &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_TWO_ATTACK) &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) &&
            !context.TryFireMainWeapon()) // Add !hastoken AS FIRST CHECK
        {
            if (context.MoveBehaviour_2 != null && context.MoveBehaviour_2.IsExecutionValid())
                context.MoveBehaviour_2.Execute();
        }
        else context.SetTargetAsPlayer(); // Prevents target being null
    }

    public void WithTokenBehaviour(BasicEnemyContext context)
    {
        if (context.AtkBehaviour_2 != null)
        {
            if (context.MyAIPath.reachedEndOfPath &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_ONE_ATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_TWO_ATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) &&
                 context.TryFireMainWeapon()) // Add hastoken AS FIRST CHECK
            {

                // Check if invoke now or wait for animation event
                if (!context.AnimExecuteAtk_2)
                {
                    if (context.AtkBehaviour_2.IsExecutionValid())
                    {
                        context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATETWOATTACK);
                        context.OnDefaultAttackBehaviour();
                        context.AtkBehaviour_2.Execute();
                    }
                }
                else
                    context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATETWOATTACK);
            }
        }
    }

    public void OnManageToken(BasicEnemyContext context)
    {
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context)
    {
        context.SetEndReachedDistance(context.WeaponManager_2.MainWeapon.Range);
    }

    public void OnStateUpdate(BasicEnemyContext context)
    {
        WithoutTokenBehaviour(context);
        WithTokenBehaviour(context);
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
