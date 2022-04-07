using UnityEngine;

public class DelegateToGameManager : MonoBehaviour
{
    [SerializeField] private bool isGargoyle = false;

    private void Start()
    {
        if (isGargoyle)
        {
            #if UNITY_WEBGL
            Destroy(gameObject);
            #endif
        }
    }

    public void DelegateQuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
