using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PlayerInputController : MonoBehaviour
{
    // SECTION - Field ============================================================
    #region REGION - Debugger Variables
    public int ColumnQty { get; set; }

    public bool OnDebugAll { get; set; }

    public Dictionary<string, bool> debugDico = new Dictionary<string, bool>
    {
        { "Jump Debug", false },
        { "Fire Main Debug", false },
        { "Reload Debug",  false },
        { "Fire Optional Debug", false },
        { "Weapon One Debug", false },
        { "Weapon Two Debug", false },
        { "Weapon Scroll <= Debug", false },
        { "Weapon Scroll => Debug", false },
        { "Interact Debug", false },
        { "Show Map Debug", false }
    };
    #endregion

    [Header("Inputs")]
    [SerializeField] private PlayerInputSO input;


    // SECTION - Method - Controller Specific ============================================================
    #region REGION - Movement
    public void OnLook(InputAction.CallbackContext cbc)
    {
        input.LookY = cbc.ReadValue<Vector2>().x;
        input.LookX = cbc.ReadValue<Vector2>().y;
    }

    public void OnMove(InputAction.CallbackContext cbc)
    {
        input.DirX = cbc.ReadValue<Vector2>().x;
        input.DirZ = cbc.ReadValue<Vector2>().y;
    }


    public void OnJump(InputAction.CallbackContext cbc)
    {
        input.Jump = cbc.performed;

        // Debugger
        if (debugDico["Jump Debug"])
            if (input.Jump)
                Debug.Log($" {gameObject.name} ... JUMP");
    }
    #endregion

    #region REGION - Weapon
    public void OnFireMainWeapon(InputAction.CallbackContext cbc)
    {
        input.FireMainWeapon = cbc.performed;

        // Debugger
        if (debugDico["Fire Main Debug"]) // onFireMainDebug
            if (input.FireMainWeapon)
                Debug.Log($" {gameObject.name} ... FIRE MAINN");
    }


    public void OnReload (InputAction.CallbackContext cbc)
    {
        input.Reload = cbc.performed;

        // Debugger
        if (debugDico["Reload Debug"])
            if(input.Reload)
                Debug.Log($" {gameObject.name} ... RELOAD");
    }


    public void OnFireOptionalWeapon(InputAction.CallbackContext cbc)
    {
        input.FireOptionalWeapon = cbc.performed;

        // Debugger
        if (debugDico["Fire Optional Debug"])
            if(input.FireOptionalWeapon)
                Debug.Log($" {gameObject.name} ... FIRE OPTIONAL");
    }

    public void OnWeaponOne(InputAction.CallbackContext cbc)
    {
        input.WeaponOne = cbc.performed;

        // Debugger
        if (debugDico["Weapon One Debug"])
            if (input.WeaponOne)
                Debug.Log($" {gameObject.name} ... CHANGE WEAPON ONE");
    }

    public void OnWeaponTwo(InputAction.CallbackContext cbc)
    {
        input.WeaponTwo = cbc.performed;

        // Debugger
        if (debugDico["Weapon Two Debug"])
            if (input.WeaponTwo)
                Debug.Log($" {gameObject.name} ... CHANGE WEAPON TWO");
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

        // DebuggerS
        if (debugDico["Weapon Scroll <= Debug"])
            if(input.WeaponScrollBackward)
                Debug.Log($" {gameObject.name} ... CHANGE WEAPON SCROLL <=");

        if (debugDico["Weapon Scroll => Debug"])
            if (input.WeaponScrollForward)
                Debug.Log($" {gameObject.name} ... CHANGE WEAPON SCROLL =>");

    }
    #endregion

    #region REGION - Misc
    public void OnInteract(InputAction.CallbackContext cbc)
    {
        input.Interact = cbc.performed;

        // Debugger
        if (debugDico["Interact Debug"])
            if (input.Interact)
                Debug.Log($" {gameObject.name} ... INTERACT");
    }

    public void OnShowMap(InputAction.CallbackContext cbc)
    {
        input.ShowMap = cbc.performed;

        // Debugger
        if (debugDico["Show Map Debug"])
            if(input.ShowMap)
                Debug.Log($" {gameObject.name} ... SHOW MAP");
    }
    #endregion


    // SECTION - Method - Debugger ============================================================
    public bool SetAllDebuggers(bool setTo)
    {
        for (int count = 0; count < debugDico.Count; count++)
            debugDico[debugDico.ElementAt(count).Key] = !setTo;

        //return debugDico["Debug All"];
        OnDebugAll = !OnDebugAll;
        return OnDebugAll;
    }
}
