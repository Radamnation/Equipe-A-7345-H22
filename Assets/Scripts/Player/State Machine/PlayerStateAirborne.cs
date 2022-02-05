using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAirborne : IPlayerState
{
    // SECTION - Method - State Specific =========================================================
    #region REGION - Movement
    public void OnLook(PlayerContext context)
    {
        context.OnDefaultLookBehaviour();
    }

    public void OnMove(PlayerContext context)
    {
        // Movement
        context.OnDefaultMovementBehaviour();
    }

    public void OnJump(PlayerContext context) { }
    #endregion

    #region REGION - Weapon
    public void OnFireWeaponMain(PlayerContext context)
    {
        if (context.Input.FireMainWeapon)
        {
            context.Input.FireMainWeapon = false;

            // EVENT GO HERE
        }
    }

    public void OnFireWeaponOptional(PlayerContext context)
    {
        if (context.Input.FireOptionalWeapon)
        {
            context.Input.FireOptionalWeapon = false;

            // EVENT GO HERE
        }
    }

    public void OnWeaponChange(PlayerContext context)
    {
        if (context.Input.WeaponOne)            // WEAPON ONE
        {
            Debug.Log($" {context.name} ... CHANGE WEAPON ONE");


            context.Input.WeaponOne = false;

            // EVENT GO HERE
        }
        else if (context.Input.WeaponTwo)       // WEAPON TWO
        {
            Debug.Log($" {context.name} ... CHANGE WEAPON TWO");


            context.Input.WeaponTwo = false;

            // EVENT GO HERE
        }
        else if (context.Input.WeaponScrollBackward)       // WEAPON SCROLL <=
        {
            Debug.Log($" {context.name} ... CHANGE WEAPON SCROLL <=");


            context.Input.WeaponScrollBackward = false;

            // EVENT GO HERE
        }
        else if (context.Input.WeaponScrollForward)       // WEAPON SCROLL =>
        {
            Debug.Log($" {context.name} ... CHANGE WEAPON SCROLL =>");


            context.Input.WeaponScrollForward = false;

            // EVENT GO HERE
        }
    }

    public void OnWeaponReload(PlayerContext context)
    {
        if (context.Input.Reload)
        {
            Debug.Log($" {context.name} ... RELOAD");


            context.Input.Reload = false;

            // EVENT GO HERE
        }
    }
    #endregion

    #region REGION - Misc
    public void OnInteract(PlayerContext context)
    {
        // Note
        //      - Whole method may need Encapsulation after completing [Interactable.cs]

        RaycastHit hit = context.TryRayCastInteractable();
        context.InteractCanvasHandler.SetActive(hit);

        if (context.Input.Interact)
        {
            context.Input.Interact = false;

            if (hit.transform != null)
            {
                hit.transform.GetComponent<Interactable>().OnInteraction();

                // NOTE FOR ITERATION
                //      - Method bellow accepts a boolean for valid or invalid interaction with interactable object
                //      - I think that the best course of action would be a boolean passed through OnInteraction()
                //        ... The boolean would be inside of the [Interactable.cs] class for logical access to the object...
                //        ... context.InteractCanvasHandler.SetVisualCue(hit.transform.GetComponent<Interactable>().OnInteraction());
                context.InteractCanvasHandler.SetVisualCue();
            }
        }
    }

    public void OnShowMap(PlayerContext context)
    {
        if (context.Input.ShowMap)
        {
            context.Input.ShowMap = false;

            // EVENT GO HERE
        }
    }
    #endregion

    // SECTION - Method - General =========================================================
    public void OnStateEnter(PlayerContext context) { context.Input.Jump = false; }

    public void OnStateUpdate(PlayerContext context)
    {
        OnLook(context);
        OnMove(context);
        //OnJump(context);

        OnFireWeaponMain(context);
        OnFireWeaponOptional(context);
        OnWeaponChange(context);
        OnWeaponReload(context);

        OnInteract(context);
        OnShowMap(context);
    }

    public IPlayerState OnStateExit(PlayerContext context)
    {
        // Dead
        if (context.LivingEntityContext.IsDead)
            return new PlayerStateDead();

        // Grounded
        if (context.TryRayCastGround().transform)
            return new PlayerStateGrounded();
          
        return this;
    }
}
