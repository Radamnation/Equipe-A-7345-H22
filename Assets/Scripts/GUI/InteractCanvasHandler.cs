using UnityEngine;
using UnityEngine.UI;

public class InteractCanvasHandler : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Animator")]
    [SerializeField] private Animator anim;
    [SerializeField] private string popUpString;
    [SerializeField] private string popOutString;

    [Header("Interact Button Backgrounds")]
    [Range(0.0f, 0.5f)][SerializeField] private float visualCueTimer = 0.1f; // Best result if <= FixedUpdate tick
    [Space(10)]
    [SerializeField] private CanvasRenderer canvasRendererBG;
    [SerializeField] private Sprite baseBackground;
    [SerializeField] private Sprite validBackground;
    [SerializeField] private Sprite invalidBackground;


    // SECTION - Method - System Specific ===================================================================
    public void SetActive(RaycastHit hit) // Fade in - Fade out [Set Animator]
    {
        if (hit.transform != null && !anim.GetCurrentAnimatorStateInfo(0).IsName(popUpString))
            anim.SetTrigger(popUpString);
        else if (hit.transform == null && !anim.GetCurrentAnimatorStateInfo(0).IsName(popOutString))
            anim.SetTrigger(popOutString);
    }

    public void SetVisualCue(bool isInteractable = true)
    {
        if (isInteractable)
            SetBackgroundSprite(validBackground);
        else
            SetBackgroundSprite(invalidBackground);

        Invoke("SetDefaultSprite", visualCueTimer);
    }


    // SECTION - Method - Utility ===================================================================
    private void SetBackgroundSprite(Sprite backgroundSprite)
    {
        canvasRendererBG.GetComponent<Image>().sprite = backgroundSprite;
    }

    private void SetDefaultSprite()
    {
        canvasRendererBG.GetComponent<Image>().sprite = baseBackground;
    }
}
