using UnityEngine;

public class BasicEnemyState_One : IEnemyState
{
    // SECTION - Field ===================================================================
    private bool launchAnimation = false;


    // SECTION - Method - State Specific ===================================================================
    public void WithoutTokenBehaviour(BasicEnemyContext context) 
    {
        context.OnDefaultSetMoveAnim();
        context.OnDefaultMoveBehaviour();

        if (!context.HasToken &&
            context.IsIddleOrMoving())
        {       
            // Behaviour
            if (context.Behaviour_NoToken_1 != null && context.Behaviour_NoToken_1.IsExecutionValid())
            {
                context.Behaviour_Token_1.Execute();

                context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATE_01_NOTOKEN);
            }              
        }
        else context.SetTargetAsPlayer(); // Prevents target being null     
    }

    public void WithTokenBehaviour(BasicEnemyContext context)
    {
        if (context.HasToken &&
            context.IsTargetNear() &&
            context.IsIddleOrMoving() ) // context.IsInRangeForAttack() &&     context.HasReachedEndOfPath() &&       
        {
            // Check: Animation based
            if (!context.AnimExecuteAtk_1)
            {
                launchAnimation = false;

                // Weapon
                if (context.TryFireMainWeapon())
                {
                    launchAnimation = true;

                    context.OnDefaultAttackBehaviour();

                    // Behaviour
                    if (context.Behaviour_Token_1 != null && context.Behaviour_Token_1.IsExecutionValid())
                    {
                        //launchAnimation = true;

                        context.Behaviour_Token_1.Execute();
                    }
                }

                // Can the animation be launched?
                if (launchAnimation)
                    context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATE_01_TOKEN);
            }
            else if ((context.Behaviour_Token_1 != null && context.Behaviour_Token_1.IsExecutionValid()) || context.IsTargetNear())//context.WeaponManager_1.IsTargetInFront())
                context.SetAnimTrigger(BasicEnemy_AnimTriggers.STATE_01_TOKEN); // Animation event based execution      
        }
    }

    public void OnManageToken(BasicEnemyContext context)
    {
        context.HasToken = true;
        //context.OnDefaultManageToken();
    }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(BasicEnemyContext context)
    {
        if (context.WeaponManager_1 != null)
            context.SetEndReachedDistance(context.WeaponManager_1.Weapon.Range);
    }

    public void OnStateUpdate(BasicEnemyContext context)
    {
        Debug.Log("Mimic in state ONE");
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
