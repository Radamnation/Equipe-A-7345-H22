using UnityEngine;
using Pathfinding;
using System.Collections;

public class DontStayIddleBehaviour : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private bool isDebugOn = false;

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
        if (myContext.HasToken && currTimer != 0.0f)
            currTimer = 0.0f;

        return myContext.IsIddleOrMoving();
            

        //return isExecutionDone && isValidForExecute;
    }


    // SECTION - Method - Behaviour Specific ===================================================================
    private void ManageTimer()
    {
        // Manage timer
        if (!myContext.MyAIPath.hasPath || (myContext.HasReachedEndOfPath() && !myContext.HasToken)) // Add to timer on iddle 
            currTimer += Time.deltaTime;
        else if (myContext.HasToken && currTimer != 0.0f)
            currTimer = 0.0f;

        // Set node
        if (currTimer >= moveAfterIddleTime )
            StartCoroutine(StartMovement());
    }

    private IEnumerator StartMovement()
    {
        isExecutionDone = false;
        isValidForExecute = false;

        StaticDebugger.SimpleDebugger(isDebugOn, $"{transform.parent.name} has entered [StartMovement()]");
        currTimer = 0.0f;
        myContext.SetSpeedAsDefault();

        // Set temp target
        //      - Either from range of node OR random node near current transform
        if (moveNearDesiredRange && myDesiredRangeOfPositions != null)
            myContext.SetTarget(myDesiredRangeOfPositions.GetRandomTransform(myContext));
        else
        {
            var point = Random.insideUnitSphere * 1.64f;
            point.y = transform.position.y;

            GraphNode node = AstarPath.active.GetNearest(transform.position + point, NNConstraint.Default).node;

            if (node != null && node.Walkable)
            {
                myContext.SetMyTemporaryTargetAs((Vector3)node.position);
                myContext.MyAIPath.destination = (Vector3)node.position;
            }
        }

        yield return new WaitForSeconds(1.0f); // Debugger, endReachedDistance always true otherwise
        yield return new WaitUntil(() => (myContext.MyAIPath.reachedEndOfPath || !myContext.HasPath()));

        if (myContext.HasToken)
        {
            myContext.SetTargetAsPlayer();
        }

        isExecutionDone = true;
        isValidForExecute = false;
    }


    private Vector3 GetValidNode()
    {
        // If there is a node, go for it
        if (moveNearDesiredRange && myDesiredRangeOfPositions != null)
            myContext.SetTarget(myDesiredRangeOfPositions.GetRandomTransform(myContext));
        //else if ()

        return Vector3.zero;

    }


    private IEnumerator StartMovement(bool a)
    {
        isExecutionDone = false;
        isValidForExecute = false;

        StaticDebugger.SimpleDebugger(isDebugOn, $"{transform.parent.name} has entered [StartMovement()]");
        currTimer = 0.0f;
        myContext.SetSpeedAsDefault();


        GraphNode nodee;
        do
        {
            nodee = AstarPath.active.GetNearest(transform.position + GetValidNode(), NNConstraint.Default).node; 

            yield return new WaitForSeconds(1.0f); // Debugger, endReachedDistance always true for attack otherwise
            yield return new WaitUntil(() => myContext.MyAIPath.reachedEndOfPath);
        } while (nodee != null && nodee.Walkable); // (!myContext.GetTargetTransform().CompareTag("Player"));







        // Set temp target
        //      - Either from range of node OR random node near current transform
        if (moveNearDesiredRange && myDesiredRangeOfPositions != null)
            myContext.SetTarget(myDesiredRangeOfPositions.GetRandomTransform(myContext));
        else
        {
            var point = Random.insideUnitSphere * 0.32f;

            int randomNegativePosition = Random.Range(0, 1);
            float newX = transform.position.z + Random.Range(0.16f, 0.32f) * randomNegativePosition == 0 ? -1 : 1;
            randomNegativePosition = Random.Range(0, 1);
            float newZ = transform.position.z + Random.Range(0.16f, 0.32f) * randomNegativePosition == 0 ? -1 : 1;
            float oldY = transform.position.y;
            point.y = oldY;
            Vector3 myNewDesiredPosition = new Vector3(newX, oldY, newZ);

            //GraphNode node = AstarPath.active.GetNearest(myNewDesiredPosition, NNConstraint.Default).node;
            //GraphNode node = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).node;
            GraphNode node = AstarPath.active.GetNearest(transform.position + point, NNConstraint.Default).node;

            if (node != null && node.Walkable)
            {
                myContext.SetMyTemporaryTargetAs((Vector3)node.position);
                myContext.MyAIPath.destination = (Vector3)node.position;
                //myContext.SetTarget(myContext.MyTemporaryTargetTransform);
            }
            /*else
            {
                newX += transform.position.x;
                newZ += transform.position.z;
                myNewDesiredPosition = new Vector3(newX, oldY, newZ);

                myContext.SetMyTemporaryTargetAs(myNewDesiredPosition);
                myContext.SetTarget(myContext.MyTemporaryTargetTransform);
            }*/
        }

        yield return new WaitForSeconds(1.0f); // Debugger, endReachedDistance always true otherwise
        yield return new WaitUntil(() => (myContext.MyAIPath.reachedEndOfPath || !myContext.HasPath()));

        if (myContext.HasToken)
        {
            myContext.SetTargetAsPlayer();
        }

        isExecutionDone = true;
        isValidForExecute = false;
    }
}
