using UnityEngine;
using Pathfinding;

public class AIManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    public static AIManager instance;
    [SerializeField] private AIPossiblePositionsAgglomerateSO positionsAgglomerate;


    // SECTION - Property ===================================================================
    public AIPossiblePositionsAgglomerateSO PositionsAgglomerate { get => positionsAgglomerate; }


    // SECTION - Method ===================================================================
    private void Start()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
}
