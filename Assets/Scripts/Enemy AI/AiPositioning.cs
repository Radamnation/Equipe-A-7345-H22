using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPositioning : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private List<Transform> myNodes;
    [SerializeField] private AIPossiblePositionsSO myReferenceSO;


    // SECTION - Property ===================================================================
    public AIPossiblePositionsSO PossiblePositionsSO { get => myReferenceSO; }


    // SECTION - Method ===================================================================
    private void Start()
    {
        myReferenceSO.PossiblePositions = myNodes;
    }
}
