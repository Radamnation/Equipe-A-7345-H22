using UnityEngine;
using Pathfinding;

public class AIManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    public static AIManager instance;
    [SerializeField] private AIPossiblePositionsAgglomerateSO positionsAgglomerate;
    [SerializeField] private AITokenHandlerSO myTokenHandlerSO;


    // SECTION - Property ===================================================================
    public AIPossiblePositionsAgglomerateSO PositionsAgglomerate { get => positionsAgglomerate; }
    public AITokenHandlerSO MyTokenHandlerSO { get => myTokenHandlerSO; }


    // SECTION - Method ===================================================================
    private void Start()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
            Destroy(this);

        MyTokenHandlerSO.ResetTokens();
    }
}
