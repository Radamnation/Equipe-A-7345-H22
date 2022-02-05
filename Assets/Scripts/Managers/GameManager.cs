using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    public static GameManager instance;

    [Header("Important Layer Masks")]
    public LayerMask groundMask;
    public LayerMask interactableMask;


    // SECTION - Method - Unity Specific ===================================================================
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        instance.SetMouseCursor_LockedInvisible();
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

}
