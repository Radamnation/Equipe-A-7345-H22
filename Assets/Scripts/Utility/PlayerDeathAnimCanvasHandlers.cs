using UnityEngine;

public class PlayerDeathAnimCanvasHandlers : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject minimapCanvas;
    [SerializeField] private GameObject weaponCanvas;
    [SerializeField] private GameObject visualFeedBack;
    [SerializeField] private OnDeathManager onDeathManager;


    // SECTION - Method - Script Specific ===================================================================
    public void Toggle_UICanvas()
    {
        if (UICanvas != null)
            UICanvas.SetActive(!UICanvas.activeSelf);
    }

    public void Toggle_MiniMapCanvas()
    {
        if (minimapCanvas != null)
            minimapCanvas.SetActive(!minimapCanvas.activeSelf);
    }

    public void Toggle_WeaponCanvas()
    {
        if (weaponCanvas != null)
            weaponCanvas.SetActive(!weaponCanvas.activeSelf);
    }

    public void Toggle_VisualFeedback()
    {
        if (weaponCanvas != null)
            visualFeedBack.SetActive(!visualFeedBack.activeSelf);
    }

    public void CheckEndAsync()
    {
        if (onDeathManager != null)
            onDeathManager.OnDeathAnimationEnd();
    }
}
