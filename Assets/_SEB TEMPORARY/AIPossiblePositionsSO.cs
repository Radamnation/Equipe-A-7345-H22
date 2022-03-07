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
    public Transform GetRandomTransform()
    {
        //var gg = AstarPath.active.data.gridGraph; // Get graph
        //var nodes = gg.nodes; // Get nodes ... Can check individually for .walkable

        // Note:
        //      - May need refactoring where a VALID node CAN be found.
        //      - Currenttly cannot find a way to get the nearest valid node, only the nearest node although it should return valid node
        Transform myDesiredTransform = null;
        for (int i = 0; i < possiblePositions.Count; i++)
        {
            myDesiredTransform = possiblePositions[Random.Range(0, possiblePositions.Count)];

            GraphNode node = AstarPath.active.GetNearest(myDesiredTransform.position, NNConstraint.Default).node;

            if (node != null && node.Walkable)
                return myDesiredTransform;
        }

        return GameManager.instance.PlayerTransformRef;
    }
}
