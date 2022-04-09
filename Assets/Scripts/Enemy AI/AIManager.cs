using UnityEngine;
using Pathfinding;

public class AIManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    public static AIManager instance;
    [SerializeField] private AIPossiblePositionsAgglomerateSO positionsAgglomerate;
    [SerializeField] private AITokenHandlerSO myTokenHandlerSO;
    private AstarPath myAstarPath;


    // SECTION - Property ===================================================================
    public AIPossiblePositionsAgglomerateSO PositionsAgglomerate { get => positionsAgglomerate; }
    public AITokenHandlerSO MyTokenHandlerSO { get => myTokenHandlerSO; }
    public AstarPath MyAstarPath { get => myAstarPath; }


    // SECTION - Method ===================================================================
    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;

            myAstarPath = GetComponent<AstarPath>();
            myAstarPath.enabled = true;

            // DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
            Destroy(this.gameObject);

        MyTokenHandlerSO.ResetTokens();
    }


    public void SetRandomTargetPosition(BasicEnemyContext myContext)
    {
        Debug.Log("Lat Resort");
        int randomNegativePosition = Random.Range(0, 1);
        float newX = myContext.transform.position.z + Random.Range(0.0f, 0.16f) * randomNegativePosition == 0 ? -1 : 1;

        randomNegativePosition = Random.Range(0, 1);
        float newZ = myContext.transform.position.z + Random.Range(0.0f, 0.16f) * randomNegativePosition == 0 ? -1 : 1;

        float oldY = myContext.transform.position.y;

        Vector3 myDesiredDestination = new Vector3(newX, oldY, newZ);




        // Set Nearest Node Constraint
        var constraint = NNConstraint.None;
        // Constrain the search to walkable nodes only
        constraint.constrainWalkability = true;
        constraint.walkable = true;
        // Constrain the search to only nodes with tag 0 (Basic Ground)
        // The 'tags' field is a bitmask
        constraint.constrainTags = true;
        constraint.tags = (1 << 0);

        var point = myContext.transform.position + Random.insideUnitSphere * 0.25f;
        point.y = myContext.transform.position.y;

        GraphNode node = AstarPath.active.graphs[(int)myContext.Type].GetNearest(point, constraint).node;

        if (node != null)
        {
            GraphNode currentPos = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myContext.transform.position, constraint).node;

            if (!PathUtilities.IsPathPossible(currentPos, node))
            {
                myContext.MyAIPath.destination = myContext.transform.localPosition + -Vector3.forward * 0.16f;
                //myContext.SetTarget();
                //myContext.SetMyTemporaryTargetAs(myContext.transform.position + -Vector3.forward * 0.16f);
            }
            else
            {
                myContext.SetMyTemporaryTargetAs((Vector3)node.position);
                myContext.SetTarget();
            }
        }
    }
}
