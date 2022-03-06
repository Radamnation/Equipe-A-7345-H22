using UnityEngine;
using Pathfinding;
using System.Collections;

public class DontStayIddleBehaviour : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Child Specific")]
    [Tooltip("If false, will move randomly around itself")]
    [SerializeField] private bool moveNearDesiredRange = true;
    [SerializeField] private AIPossiblePositionsSO myDesiredRangeOfPositions;
    [SerializeField] private float moveAfterIddleTime = 1.0f;
                     private float currTimer = 0.0f;


    // SECTION - Method - Unity Specific ===================================================================
    public void FixedUpdate()
    {
        Behaviour();
    }

    private void Start()
    {
        SetMyBasicEnemyContext();
    }


    // SECTION - Method - Implementation ===================================================================
    public override void Behaviour()
    {
        ManageTimer();
    }

    public override bool ChildSpecificValidations()
    {
        throw new System.NotImplementedException();
    }


    // SECTION - Method - Behaviour Specific ===================================================================
    private void ManageTimer()
    {
        if (!myContext.MyAIPath.hasPath)                           // Add to timer on iddle 
            currTimer += Time.deltaTime;

        // Set node
        if (currTimer >= moveAfterIddleTime )
            StartCoroutine(StartMovement());
    }

    private IEnumerator StartMovement()
    {
        Debug.Log("STARTED MOVEMENT");
        currTimer = 0.0f;
        myContext.SetSpeedAsDefault();

        // Set temp target
        //      - Either from range of node OR random node near current transform
        if (moveNearDesiredRange && myDesiredRangeOfPositions != null)
            myContext.SetTarget(myDesiredRangeOfPositions.GetRandomTransform());
        else
        {
            GraphNode node = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;

            if (node != null && node.Walkable)
            {
                myContext.SetMyTemporaryTargetAs((Vector3)node.position);
                myContext.SetTarget(myContext.MyTemporaryTargetTransform);
            }
        }

        yield return new WaitForSeconds(0.5f); // Debugger, endReachedDistance always true otherwise
        yield return new WaitUntil(() => myContext.MyAIPath.reachedEndOfPath);

        myContext.SetTargetAsPlayer();
    }
}
