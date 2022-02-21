using UnityEngine;

public class SetStateNearTarget : AbstractMovementBehaviour
{
    // SECTION - Field ============================================================
    private BasicEnemyContext myContext;

    [Header("Important Variables")]
    [SerializeField] private bool isPassiveBehaviour = false;
    [SerializeField] private BasicEnemy_States nextState;
    [SerializeField] private LayerMask waitForMask;
    [SerializeField] private float waitUntilRange;


    // SECTION - Method - Unity Specific ============================================================
    private void Start()
    {
        myContext = GetComponentInParent<BasicEnemyContext>();
    }

    private void FixedUpdate()
    {
        if (isPassiveBehaviour)
            Execute();
    }


    // SECTION - Method - Implementation Specific ============================================================
    public override void Execute()
    {

        Collider[] hits = StaticRayCaster.IsOverlapSphereTouching
                        (myContext.transform, waitUntilRange, waitForMask);

        foreach (Collider hit in hits) // May need debug : this.entity takes itself into account even when conditioning against
        {
            if (hit.transform != null && hit.name != transform.parent.name)
            {
                // Check for "Iddle ambush"
                if (myContext.IsInAnimationState(BasicEnemy_AnimationStates.IDDLE))
                    myContext.SetAnimTrigger(BasicEnemy_AnimTriggers.ONHIT); // -> Awake

                // Set new state
                if (!myContext.MyLivingEntity.IsDead)
                    myContext.SetFiniteStateMachine(nextState);
                break;
            }
        }
    }
}
