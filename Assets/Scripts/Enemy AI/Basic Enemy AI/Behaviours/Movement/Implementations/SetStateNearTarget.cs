using UnityEngine;

public class SetStateNearTarget : AbstractMovementBehaviour
{
    // SECTION - Field ============================================================
    [Header("Important Variables")]
    [SerializeField] private bool isPassiveBehaviour = false;
    [SerializeField] private BasicEnemy_States nextState;
    [SerializeField] private LayerMask waitForMask;
    [SerializeField] private float waitUntilRange;


    // SECTION - Method - Unity Specific ============================================================
    private void FixedUpdate()
    {
        if (isPassiveBehaviour)
            Execute();
    }


    // SECTION - Method - Implementation Specific ============================================================
    public override void Execute()
    {

        Collider[] hits;
        

            hits = StaticRayCaster.IsOverlapSphereTouching
                        (MyContext.transform, waitUntilRange, waitForMask); // MyContext.transform


        foreach (Collider hit in hits) // May need debug : this.entity takes itself into account even when conditioning against
        {
            if (hit.transform != null && hit.name != transform.parent.name)
            {
                // Check for "Iddle ambush"
                if (MyContext.IsInAnimationState(BasicEnemy_AnimationStates.IDDLE))
                    MyContext.SetAnimTrigger(BasicEnemy_AnimTriggers.ONHIT); // -> Awake

                // Set new state
                if (!MyContext.MyLivingEntity.IsDead)
                    MyContext.SetFiniteStateMachine(nextState);
                break;
            }
        }
    }
}
