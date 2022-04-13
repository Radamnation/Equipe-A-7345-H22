using UnityEngine;
using Pathfinding;
using System.Collections;

public class DontStayIddleBehaviour : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Child Specific")]
    [SerializeField] private Vector3[] myDesiredRangedOfPosition = new Vector3[]
    {
        new Vector3(0.96f, 0.0f, 0.0f),
        new Vector3(-0.96f, 0.0f, 0.0f),

        new Vector3(0.0f, 0.0f, -0.96f),
        new Vector3(0.0f, 0.0f, 0.96f),

        new Vector3(-0.64f, 0.0f, -0.64f),
        new Vector3(0.64f, 0.0f, -0.64f),
        new Vector3(-0.64f, 0.0f, 0.64f),
        new Vector3(0.64f, 0.0f, 0.64f)
    };

    [SerializeField] private AIPossiblePositionsSO myDesiredRangeOfPositions;
    [SerializeField] private float moveAfterIddleTime = 1.0f;
                     private float currTimer = 0.0f;

    NNConstraint nodeConstraint = null;


    // SECTION - Method - Unity Specific ===================================================================
    public void FixedUpdate()
    {
        if (isPassive)
            Execute();



        var point = myContext.transform.position + Random.insideUnitSphere;
        point.y = myContext.transform.position.y;

        //Debug.Log("POINT IS:" + point);
    }


    // SECTION - Method - Implementation ===================================================================
    public override void Behaviour()
    {
        DontStayIddle();
    }

    public override bool ChildSpecificValidations()
    {
        /*
        Debug.Log("ENTERED Dont stay iddle");
        if (TrySetOverlapSphereHits())
        {
            myContext.SetTargetAsPlayer(); // TEMPORARY - MAKE IT SO IT IS GENERIC-ISH
            return false;
        }

        // Reset Timer
        //if ((myContext.HasToken || myContext.HasPath()) && currTimer != 0.0f)
        if (myContext.HasPath() && currTimer != 0.0f)
            currTimer = 0.0f;

        // Manage timer
        if (!myContext.HasPath()) // Add to timer on iddle 
            currTimer += Time.deltaTime;

        return myContext.IsIddleOrMoving() && currTimer >= moveAfterIddleTime;*/

        if (nodeConstraint == null)
        {
            // Set Nearest Node Constraint
            nodeConstraint = NNConstraint.None;
            // Constrain the search to walkable nodes only
            nodeConstraint.constrainWalkability = true;
            nodeConstraint.walkable = true;
            // Constrain the search to only nodes with tag 0 (Basic Ground)
            // The 'tags' field is a bitmask
            nodeConstraint.constrainTags = true;
            nodeConstraint.tags = (1 << 0);
        }

        // Check for nearest node from last DesiredTransform with Seeker
        GraphNode node_Seeker = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myContext.GetTargetTransform().position, nodeConstraint).node;
        GraphNode currentPoss_Seeker = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myContext.transform.position, nodeConstraint).node;
        Debug.Log($"Is path possible? {PathUtilities.IsPathPossible(currentPoss_Seeker, node_Seeker)}");
        return PathUtilities.IsPathPossible(currentPoss_Seeker, node_Seeker);

    }


    // SECTION - Method - Behaviour Specific ===================================================================
    private void DontStayIddle()
    {
        Debug.Log($"timer On Execute is: {currTimer}");

        // Reset Timer on behaviour use
        currTimer = 0.0f;

        // Set Target
        StartCoroutine(StartMovement());
    }

    private IEnumerator StartMovement()
    {
        Debug.Log("Dont stay iddle started coroutine");

        myContext.SetTarget();

        for (int i = 0; i < myDesiredRangedOfPosition.Length * 2; i++)
        {
            int rand = Random.Range(0, myDesiredRangedOfPosition.Length);
                
            // Check for nearest node from last DesiredTransform with Seeker
            GraphNode node_Seeker = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myDesiredRangedOfPosition[rand], nodeConstraint).node;
            GraphNode currentPoss_Seeker = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myContext.transform.position, nodeConstraint).node;

            if (PathUtilities.IsPathPossible(currentPoss_Seeker, node_Seeker))
            {
                Debug.Log("Node found!");
                myContext.SetTarget((Vector3)node_Seeker.position);
            }
        }

        // Last resort
        // Check for nearest node from last DesiredTransform with Seeker
        GraphNode node_Seekerr = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myDesiredRangedOfPosition[Random.Range(0, myDesiredRangedOfPosition.Length)], nodeConstraint).node;
        GraphNode currentPoss_Seekerr = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myContext.transform.position, nodeConstraint).node;

        if (!PathUtilities.IsPathPossible(currentPoss_Seekerr, node_Seekerr))
        {
            myContext.GetComponent<Seeker>().StartPath((Vector3)currentPoss_Seekerr.position, (Vector3)node_Seekerr.position, OnPathCalculated);
            myContext.SetTarget();
            myContext.SetMyTemporaryTargetAs(temp);
        }

        yield return new WaitForSeconds(1.0f); // Debugger, endReachedDistance always true for attack otherwise
        yield return new WaitUntil(() => myContext.HasReachedEndOfPath());

        myContext.SetTargetAsPlayer();

        myContext.CanUseBehaviour = true;
        isValidForExecute = false;
    }


    Vector3 temp = new Vector3();
    private void OnPathCalculated(Path p)
    {
        if (p.error)
        {
            return;
        }

        temp = (Vector3)p.path[0].position;
        Debug.Log((Vector3)p.path[0].position);
    }
}