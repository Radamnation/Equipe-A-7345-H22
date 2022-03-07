using UnityEngine;

public class BasicEnemyState_Two : IEnemyState
{
    // SECTION - Field ===================================================================
    private bool launchAnimation = false;


    // SECTION - Method - State Specific ===================================================================
    public void WithoutTokenBehaviour(BasicEnemyContext context)
    {
        context.OnDefaultSetMoveAnim();
        context.OnDefaultMoveBehaviour();

        if (!context.HasToken &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_ONE_ATTACK) &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_TWO_ATTACK) &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) )
        {
            // Behaviour
            if (context.Behaviour_NoToken_2 != null && context.Behaviour_NoToken_2.IsExecutionValid())
            {
                context.Behaviour_Token_2.Execute();

                //context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATEONEATTACK);
            }
        }
        else context.SetTargetAsPlayer(); // Prevents target being null
    }

    public void WithTokenBehaviour(BasicEnemyContext context)
    {
        if (context.HasToken &&
            context.MyAIPath.reachedEndOfPath &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_ONE_ATTACK) &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.STATE_TWO_ATTACK) &&
            !context.IsInAnimationState(BasicEnemy_AnimationStates.ONAWAKE) )
        {
            // Check: Animation based
            if (!context.AnimExecuteAtk_2)
            {
                launchAnimation = false;

                // Weapon
                if (context.TryFireMainWeapon())
                {
                    launchAnimation = true;

                    context.OnDefaultAttackBehaviour();

                    // Behaviour
                    if (context.Behaviour_Token_2 != null && context.Behaviour_Token_2.IsExecutionValid())
                    {
                        //launchAnimation = true;

                        context.Behaviour_Token_2.Execute();
                    }
                }

                // Can the animation be launched?
                if (launchAnimation)
                    context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATETWOATTACK);
            }
            else if ((context.Behaviour_Token_2 != null && context.Behaviour_Token_2.IsExecutionValid()) || context.WeaponManager_2.IsTargetInFront())
                context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATETWOATTACK); // Animation event based execution    
        }
    }

    public void OnManageToken(BasicEnemyContext context)
    {
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context)
    {
        context.SetEndReachedDistance(context.WeaponManager_2.Weapon.Range);
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
