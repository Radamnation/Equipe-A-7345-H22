using UnityEngine;

public class OnDeathManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private PlayerInputSO playerInputSO;


    // SECTION - Method - Unity Specific ===================================================================
    void Update()
    {
        // On Any Key, load scene (should load hub instead, when implemented)
        if (playerInputSO.AnyKey)
            GameManager.instance.LoadScene(0);
    }
}
