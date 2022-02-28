using UnityEngine;

public class SetStateNearTarget : AbstractBehaviour
{
    // SECTION - Field ============================================================
    [Header("Important Variables")]
    [SerializeField] private bool isPassiveBehaviour = false;
    [SerializeField] private BasicEnemy_States nextState;

    private Collider[] hits;


    // SECTION - Method - Unity Specific ============================================================
    private void FixedUpdate()
    {
        if (isPassiveBehaviour && IsExecutionValid())
            Execute();
    }


    // SECTION - Method - Implementation Specific ============================================================
    public override void Behaviour()
    {
        // Check for "Iddle ambush"
        if (myContext.IsInAnimationState(BasicEnemy_AnimationStates.IDDLE))
            myContext.SetAnimTrigger(BasicEnemy_AnimTriggers.ONHIT); // -> Awake

        // Set new state
        if (!myContext.MyLivingEntity.IsDead)
            myContext.SetFiniteStateMachine(nextState);
    }

    public override bool ChildSpecificValidations()
    {
        hits = StaticRayCaster.IsOverlapSphereTouching
                    (myContext.transform, distance, targetMask);

        foreach (Collider hit in hits) // May need debug : this.entity takes itself into account even when conditioning against
            if (hit.transform != null && hit.name != transform.parent.name)
                return true;

        return false;
    }
}
