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
}
