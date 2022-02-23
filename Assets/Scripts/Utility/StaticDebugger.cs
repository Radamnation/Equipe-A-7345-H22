using UnityEngine;

public class StaticDebugger : MonoBehaviour
{
    // SECTION - Method =========================================================
    static public void SimpleDebugger(bool isDebugOn, string debugMessage)
    {
        if (isDebugOn)
            Debug.Log(debugMessage);
    }
}
