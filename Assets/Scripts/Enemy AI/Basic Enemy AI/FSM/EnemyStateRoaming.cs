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
            !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE))
        {       
            if (context.R_MoveBehaviour != null)
                context.R_MoveBehaviour.Execute();
        }
        else context.SetTargetAsPlayer(); // Prevents target being null     
    }

    public void OnAttack(BasicEnemyContext context)
    {
        if (context.R_AtkBehaviour != null)
        {
            if (context.MyAIPath.reachedEndOfPath &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ROAMINGATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.AGGRESSIVEATTACK) &&
                !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE)) // Add && canAttackCooldown check here
            {
                // Start canAttack cooldown here
                Debug.Log("ROAMING : IMPLEMENT ATTACK COOLDOWN HERE");

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
        Debug.Log("ROAMING : FINISH IMPLEMENTATION HERE");
        //context.MyAIPath.endReachedDistance = context.MyRoamingWeaponHolder.weapon.range;
    }

    public void OnStateUpdate(BasicEnemyContext context)
    {
        OnMovement(context);
        OnAttack(context);
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
