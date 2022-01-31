using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{
    // SECTION - Field ============================================================
    [Header("Inputs")]
    [SerializeField] private PlayerInputSO input;

    /*
    [Header("UI Sprites")]
    [SerializeField] private CanvasRenderer[] uiCanvases;
    [SerializeField] private Sprite[] keyboardSprites;
    [SerializeField] private Sprite[] gamepadSprites;
    */

    // SECTION - Method - Controller Specific ============================================================
    #region REGION - Movement
    public void OnLook(InputAction.CallbackContext cbc)
    {
        input.LookY = cbc.ReadValue<Vector2>().x;
        input.LookX = cbc.ReadValue<Vector2>().y;

        SetKeyLayout(cbc); // Change UI based on controller type
    }

    public void OnMove(InputAction.CallbackContext cbc)
    {
        input.DirX = cbc.ReadValue<Vector2>().x;
        input.DirZ = cbc.ReadValue<Vector2>().y;

        SetKeyLayout(cbc); // Change UI based on controller type
    }


    public void OnJump(InputAction.CallbackContext cbc)
    {
        input.Jump = cbc.performed;

        SetKeyLayout(cbc); // Change UI based on controller type
    }
    #endregion

    #region REGION - Weapon
    public void OnFireMainWeapon(InputAction.CallbackContext cbc)
    {
        input.FireMainWeapon = cbc.performed;

        SetKeyLayout(cbc); // Change UI based on controller type
    }


    public void OnReload (InputAction.CallbackContext cbc)
    {
        input.Reload = cbc.performed;

        SetKeyLayout(cbc); // Change UI based on controller type
    }


    public void OnFireOptionalWeapon(InputAction.CallbackContext cbc)
    {
        input.FireOptionalWeapon = cbc.performed;

        SetKeyLayout(cbc); // Change UI based on controller type
    }

    public void OnWeaponOne(InputAction.CallbackContext cbc)
    {
        input.WeaponOne = cbc.performed;

        SetKeyLayout(cbc); // Change UI based on controller type
    }

    public void OnWeaponTwo(InputAction.CallbackContext cbc)
    {
        input.WeaponTwo = cbc.performed;

        SetKeyLayout(cbc); // Change UI based on controller type
    }

    public void OnWeaponScrollBack(InputAction.CallbackContext cbc)
    {
        // Note
        //      - Weird bug where [y -Mouse Scroll-] gets a value, but [input.WeaponScrollBack] isn't updated?

        // Value check is based on:
        //      - Y : Mouse Scroll
        //      - X : D-Pad Left & Right

        if (cbc.ReadValue<Vector2>().y != 0) 
        {
            // <= : Scroll Backward
            input.WeaponScrollBackward = cbc.ReadValue<Vector2>().y < 0 || cbc.ReadValue<Vector2>().x < 0;

            // => : Scroll Forward
            input.WeaponScrollForward = cbc.ReadValue<Vector2>().y > 0 || cbc.ReadValue<Vector2>().x > 0;
        }
    }
    #endregion

    #region REGION - Misc
    public void OnInteract(InputAction.CallbackContext cbc)
    {
        input.Interact = cbc.performed;

        SetKeyLayout(cbc); // Change UI based on controller type
    }

    public void OnShowMap(InputAction.CallbackContext cbc)
    {
        input.ShowMap = cbc.performed;

        SetKeyLayout(cbc); // Change UI based on controller type
    }
    #endregion


    // SECTION - Method - Utility ============================================================
    public void SetKeyLayout(InputAction.CallbackContext cbc) // From old project -Sebastien Levesque "Push" Game Homework-
    {
        /*
        if (uiCanvases != null)
        {
            // Keyboard
            if (InputControlPath.MatchesPrefix("<Keyboard>", cbc.control))
                for (int i = 0; i < uiCanvases.Length; i++)
                    uiCanvases[i].GetComponent<Image>().sprite = keyboardSprites[i];


            // Gamepad
            if (InputControlPath.MatchesPrefix("<GamePad>", cbc.control))
                for (int i = 0; i < uiCanvases.Length; i++)
                    uiCanvases[i].GetComponent<Image>().sprite = gamepadSprites[i];
        }
        */



        /*
        // Keyboard
        if (InputControlPath.MatchesPrefix("<Keyboard>", cbc.control))
            uiSprite.GetComponent<Image>().sprite = possibleUiSprites[0];

        // Gamepad
        else if (InputControlPath.MatchesPrefix("<GamePad>", cbc.control))
            uiSprite.GetComponent<Image>().sprite = possibleUiSprites[1];
        */
    }
}
