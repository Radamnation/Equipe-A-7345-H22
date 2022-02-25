using UnityEngine;

public class PlayerDeathAnimCanvasHandlers : MonoBehaviour
{
    // SECTION - Field ===================================================================
    private GameObject UICanvas;
    private GameObject weaponCanvas;
    private GameObject visualFeedBack;
    private OnDeathManager onDeathManager;


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        UICanvas = GameObject.Find("UI Canvas").gameObject;
        weaponCanvas = GameObject.Find("Weapon Canvas").gameObject;
        visualFeedBack = GameObject.Find("Visual Feedback Canvas").gameObject;

        onDeathManager = GameObject.Find("Death Canvas").GetComponent<OnDeathManager>();
    }


    // SECTION - Method - Script Specific ===================================================================
    public void Toggle_UICanvas()
    {
        UICanvas.SetActive(!UICanvas.activeSelf);
    }

    public void Toggle_WeaponCanvas()
    {
        weaponCanvas.SetActive(!weaponCanvas.activeSelf);
    }

    public void Toggle_VisualFeedback()
    {
        visualFeedBack.SetActive(!visualFeedBack.activeSelf);
    }

    public void CheckEndAsync()
    {
        onDeathManager.OnDeathAnimationEnd();
    }
}
