using UnityEngine;

public class DelegateToGameManager : MonoBehaviour
{
    public void DelegateQuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
