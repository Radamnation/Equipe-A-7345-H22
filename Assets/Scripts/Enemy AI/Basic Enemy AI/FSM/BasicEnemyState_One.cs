using UnityEngine;

public class BasicEnemyState_One : IEnemyState
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
            if (context.MoveBehaviour_1 != null && context.MoveBehaviour_1.IsExecutionValid())
                context.MoveBehaviour_1.Execute();
        }
        else context.SetTargetAsPlayer(); // Prevents target being null     
    }


    /*
                 if (context.MyAIPath.reachedEndOfPath &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_ONE_ATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_TWO_ATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) &&
                 context.TryFireMainWeapon()) // Add hastoken AS FIRST CHECK
     */

    public void WithTokenBehaviour(BasicEnemyContext context)
    {
        if (context.AtkBehaviour_1 != null)
        {
            if (context.MyAIPath.reachedEndOfPath &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_ONE_ATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_TWO_ATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) ) // Add hastoken AS FIRST CHECK
            {
                // Check if invoke now or wait for animation event
                if (!context.AnimExecuteAtk_1)
                {
                    if (context.TryFireMainWeapon() && context.AtkBehaviour_1.IsExecutionValid())
                    {
                        context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATEONEATTACK);
                        context.OnDefaultAttackBehaviour();
                        context.AtkBehaviour_1.Execute();
                    }
                }
                else
                    context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATEONEATTACK);          
            }
        }
    }

    public void OnManageToken(BasicEnemyContext context)
    {
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context)
    {
        context.SetEndReachedDistance(context.WeaponManager_1.MainWeapon.Range);
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
