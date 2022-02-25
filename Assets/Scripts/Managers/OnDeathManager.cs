using UnityEngine;
using TMPro;

public class OnDeathManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private string loadingText = "Loading...";
    [SerializeField] private int hubSceneInt = 0;
    [SerializeField] private PlayerInputSO playerInputSO;
    private GameObject myLoadSceneEndCue;

    private bool hasAsyncEndedWithAnim = true;


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        // Get Component
        myLoadSceneEndCue = transform.GetChild(0).gameObject;

        // Start Load Hub uppon entering [PlayerStateDead.cs]
        GameManager.instance.LoadSceneAsync(hubSceneInt); // TO BE CHANGED FOR : GameManager.instance.StringHUB & delete hubsceneint
    }


    void Update()
    {
        // Print textual cue for end of loading
        if (!hasAsyncEndedWithAnim)
            OnLoadAsyncEndShowCue();

        // On Any Key, load scene
        if (myLoadSceneEndCue.gameObject.activeSelf == true && playerInputSO.AnyKey)
        {
            myLoadSceneEndCue.GetComponent<TextMeshProUGUI>().text = loadingText;
            GameManager.instance.LoadScene(hubSceneInt);
        }
    }


    // SECTION - Method - Utility ===================================================================
    public void OnDeathAnimationEnd()
    {
        if (GameManager.instance.AsyncLoad == null)
            GameManager.instance.LoadSceneAsync(0); // TO BE CHANGED FOR : GameManager.instance.StringHUB & delete hubsceneint

        if (GameManager.instance.AsyncLoad.progress >= 0.9f)
            myLoadSceneEndCue.gameObject.SetActive(true);
        else
            hasAsyncEndedWithAnim = false;
    }

    public void OnLoadAsyncEndShowCue() // Allows for load scene uppon async progress >= 0.9f
    {
        if (GameManager.instance.AsyncLoad.progress >= 0.9f)
            myLoadSceneEndCue.gameObject.SetActive(true);
    }
}
