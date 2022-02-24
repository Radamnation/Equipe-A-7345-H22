using UnityEngine;

public class SetActiveUI : MonoBehaviour
{
    // SECTION - Field ===================================================================
    private GameObject playerGUI;
    private GameObject UICanvas;
    private GameObject weaponCanvas;
    private GameObject visualFeedBack;


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        playerGUI = GameObject.Find("--------------------- GUI").gameObject;

        UICanvas = GameObject.Find("UI Canvas").gameObject;
        weaponCanvas = GameObject.Find("Weapon Canvas").gameObject;
        visualFeedBack = GameObject.Find("Visual Feedback Canvas").gameObject;
    }


    // SECTION - Method - Script Specific ===================================================================
    public void SetActiveAll(bool setAs)
    {
        playerGUI.SetActive(setAs);
    }

    public void SetActive_UICanvas(bool setAs)
    {
        UICanvas.SetActive(setAs);
    }

    public void Toggle_UICanvas()
    {
        UICanvas.SetActive(!UICanvas.activeSelf);
    }

    public void SetActive_WeaponGUI(bool setAs)
    {
        weaponCanvas.SetActive(setAs);
    }

    public void Toggle_WeaponCanvas()
    {
        weaponCanvas.SetActive(!weaponCanvas.activeSelf);
    }

    public void SetActive_VisualFeedback(bool setAs)
    {
        visualFeedBack.SetActive(setAs);
    }

    public void Toggle_VisualFeedback()
    {
        visualFeedBack.SetActive(!visualFeedBack.activeSelf);
    }
}
