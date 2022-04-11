using UnityEngine;

public class AB_ManageOnDeathAnim : StateMachineBehaviour
{
    // SECTION - Field ============================================================
    private LivingEntityContext myContext;


    // SECTION - Method ============================================================
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get BasicEnemyContext
        if (myContext == null)
            myContext = animator.GetComponent<LivingEntityContext>();
        if (myContext == null)
            myContext = animator.GetComponentInParent<LivingEntityContext>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myContext != null)
            myContext.AE_ManageObjectAtEndDeathAnim();
    }
}
