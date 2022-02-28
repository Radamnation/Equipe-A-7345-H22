using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/AI/AI Possible Positions Agglomerate", fileName = "AI Possible Positions Agglomerate SO")]
public class AIPossiblePositionsAgglomerateSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    public AIPossiblePositionsSO frontClosePositions;
    public AIPossiblePositionsSO frontMidPositions;
    public AIPossiblePositionsSO backPositions;
}
