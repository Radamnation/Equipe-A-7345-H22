using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAirborne : IPlayerState
{
    // SECTION - Method - State Specific =========================================================
    #region REGION - Movement
    public void OnLook(PlayerContext context)
    {
        float lookY = context.Input.LookY * context.Input.MouseSensitivity.Value;

        Vector3 rotationValues = Vector3.up * lookY;

        context.transform.Rotate(rotationValues);
    }

    public void OnMove(PlayerContext context)
    {
        // Movement
        float moveX = context.Input.DirX * context.Input.MoveFactor.Value;
        float moveZ = context.Input.DirZ * context.Input.MoveFactor.Value;

        Vector3 movement = context.transform.right * moveX + 
                            context.transform.up * context.Rb.velocity.y +
                            context.transform.forward * moveZ;

        context.Rb.velocity = movement;

        // Animator
        // SET ANIMATOR HERE
    }

    public void OnJump(PlayerContext context) { }
    #endregion

    #region REGION - Weapon
    public void OnFireWeaponMain(PlayerContext context)
    {
        if (context.Input.FireMainWeapon)
        {
            Debug.Log($" {context.name} ... FIRE MAIN");


            context.Input.FireMainWeapon = false;

            // EVENT GO HERE
        }
    }

    public void OnFireWeaponOptional(PlayerContext context)
    {
        if (context.Input.FireOptionalWeapon)
        {
            Debug.Log($" {context.name} ... FIRE OPTIONAL");


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
        if (context.Input.Interact)
        {
            Debug.Log($" {context.name} ... INTERACT");


            context.Input.Interact = false;

            // NOTE
            //      - condition only as a temporary template
            if (StaticRayCaster.IsTouching(context.transform.position, context.transform.forward, context.DistanceInteractible, context.MaskGround, context.IsDebugOn).transform)
            {
                // EVENT GO HERE
            }
        }
    }

    public void OnShowMap(PlayerContext context)
    {
        if (context.Input.ShowMap)
        {
            Debug.Log($" {context.name} ... SHOW MAP");


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
        // Grounded
        if (StaticRayCaster.IsTouching(context.transform.position,
                                        -context.transform.up,
                                         context.DistanceGround,
                                         context.MaskGround,
                                         context.IsDebugOn).transform)
            return new PlayerStateGrounded();
          
        return this;
    }
}
