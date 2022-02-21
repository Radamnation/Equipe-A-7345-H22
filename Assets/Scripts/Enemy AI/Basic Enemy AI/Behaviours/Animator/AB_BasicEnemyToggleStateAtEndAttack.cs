using UnityEngine;

public class AB_BasicEnemyToggleStateAtEndAttack : StateMachineBehaviour
{
    // SECTION - Field ============================================================
    private BasicEnemyContext myContext;


    // SECTION - Method ============================================================
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get BasicEnemyContext
        if (myContext == null)
            myContext = animator.GetComponent<BasicEnemyContext>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Toggle handler
        if (myContext.IsInAnimationState(BasicEnemy_AnimationStates.ROAMINGATTACK) && myContext.R_OnAnimExit)
            myContext.ToggleState();
        else if (myContext.IsInAnimationState(BasicEnemy_AnimationStates.AGGRESSIVEATTACK) && myContext.A_OnAtkExit)
            myContext.ToggleState();
    }
}
