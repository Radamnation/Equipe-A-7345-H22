using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[CreateAssetMenu(menuName = "Scriptable/AI/AI Possible Positions", fileName = "AI Possible Positions SO")]
public class AIPossiblePositionsSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    [SerializeField] private List<Transform> possiblePositions = new List<Transform>();


    // SECTION - Property ===================================================================
    public List<Transform> PossiblePositions { get => possiblePositions; set => possiblePositions = value; }


    // SECTION - Method ===================================================================
    private void OnEnable()
    {
        
    }
    /*
    public Transform GetRandomTransform(BasicEnemyContext context)
    {
        //var gg = AstarPath.active.data.gridGraph; // Get graph
        //var nodes = gg.nodes; // Get nodes ... Can check individually for .walkable

        Transform myDesiredTransform = null;
        for (int i = 0; i < possiblePositions.Count; i++)
        {
            myDesiredTransform = possiblePositions[Random.Range(0, possiblePositions.Count)];

            GraphNode node = AstarPath.active.GetNearest(myDesiredTransform.position, NNConstraint.Default).node;

            context.SetTarget(myDesiredTransform);

            
            if (node != null && node.Walkable)
                return myDesiredTransform;
            
        }

        // If absolutely nothing is found
        return GameManager.instance.PlayerTransformRef;
    }

    */


    ///*
    public void SetRandomTargetPosition(BasicEnemyContext myContext)
    {
        //var gg = AstarPath.active.data.graphs[1]; // Get graph [1]
       // var nodes = gg.GetNearest; // Get nodes ... Can check individually for .walkable

        // Set Nearest Node Constraint
        var constraint = NNConstraint.None;
        // Constrain the search to walkable nodes only
        constraint.constrainWalkability = true;
        constraint.walkable = true;
        // Constrain the search to only nodes with tag 0 (Basic Ground)
        // The 'tags' field is a bitmask
        constraint.constrainTags = true;
        constraint.tags = (1 << 0);

        // Check for nodes
        Transform myDesiredTransform = null;
        for (int i = 0; i < possiblePositions.Count * 2; i++)
        {
            myDesiredTransform = possiblePositions[Random.Range(0, possiblePositions.Count)];

            GraphNode node = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myDesiredTransform.position, constraint).node;

            if (node != null)
            {
                myContext.SetMyTemporaryTargetAs((Vector3)node.position);
                myContext.SetTarget();

                GraphNode currentPos = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myContext.transform.position, constraint).node;
                if (PathUtilities.IsPathPossible(currentPos, node))
                    return;
            }
        }

        // Check for nearest node from last DesiredTransform with Seeker
        GraphNode node_Seeker = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myDesiredTransform.position, constraint).node;
        GraphNode currentPoss_Seeker = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myContext.transform.position, constraint).node;
        if (!PathUtilities.IsPathPossible(currentPoss_Seeker, node_Seeker))
        {
            myContext.GetComponent<Seeker>().StartPath((Vector3)currentPoss_Seeker.position, (Vector3)node_Seeker.position, OnPathCalculated);
            myContext.SetTarget();
            myContext.SetMyTemporaryTargetAs(temp);

            if (myContext.HasPath())
                return;
        }

        // Where all fails, try that
        if (!PathUtilities.IsPathPossible(currentPoss_Seeker, node_Seeker))
            AIManager.instance.SetRandomTargetPosition(myContext);
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

    //*/
}
