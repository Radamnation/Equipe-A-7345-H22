using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    public static GameManager instance;

    [Header("Important Layer Masks")]
    public LayerMask groundMask;
    public LayerMask interactableMask;
    public LayerMask canBeShotByPlayerMask;
    public LayerMask respawnMask;

    [Header("Important Scenes")]
    [SerializeField] private string stringHUB = "HUB";

    private Transform playerTransformRef;

    private AsyncOperation asyncLoad;


    // SECTION - Property ===================================================================
    public Transform PlayerTransformRef => playerTransformRef;

    public AsyncOperation AsyncLoad { get => asyncLoad; }
    public string StringHUB { get => stringHUB; }


    // SECTION - Method - Unity Specific ===================================================================
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        instance.SetMouseCursor_LockedInvisible();

        playerTransformRef = GameObject.Find("Player").transform;
    }


    // SECTION - Method - Utility ===================================================================
    #region REGION - Mouse Cursor & Mouse Visible
    public void ToggleMouseCursor_ConfinedToLocked()
    {
        Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
    }

    public void SetMouseCursor_LockedInvisible()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetMouseCursor_Manual(CursorLockMode lockMode, bool cursorVisible)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = cursorVisible;
    }
    #endregion


    #region REGION - Scene Load & Quit
    // Basic
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(scene).name);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Async
    public void LoadSceneAsync(int scene, bool allowSceneActivation = false)
    {
        // Async load desired scene
        asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = allowSceneActivation;

        Debug.Log($"asyncload: {asyncLoad}");

        // Prevents unintentional inputs
        Input.ResetInputAxes();

        // Garbage collection - just in case -
        System.GC.Collect();
    }

    public void LoadSceneAsync(string scene, bool allowSceneActivation = false)
    {
        // Async load desired scene
        asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = allowSceneActivation;

        // Prevents unintentional inputs
        Input.ResetInputAxes();

        // Garbage collection - just in case -
        System.GC.Collect();
    }

    // Quit Game
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
