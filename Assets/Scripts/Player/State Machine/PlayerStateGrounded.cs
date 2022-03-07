using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* REFERENCE if change of mind for delegation setup
if (context.Input.FireMainWeapon)
{
    context.Input.FireMainWeapon = false;
    // EVENT GOES HERE
}
*/

public class PlayerStateGrounded : IPlayerState
{
    // SECTION - Method - State Specific =========================================================
    #region REGION - Movement
    public void OnLook(PlayerContext context)
    {
        context.OnDefaultLook();
    }

    public void OnMove(PlayerContext context)
    {
        context.OnDefaultMovement();
    }

    public void OnJump(PlayerContext context)
    {
        if (context.Input.Jump)
        {
            float moveX = context.Input.DirX * context.Input.MoveFactor.Value;
            float moveY = context.Input.JumpFactor.Value;
            float moveZ = context.Input.DirZ * context.Input.MoveFactor.Value;

            Vector3 movement = context.transform.right * moveX + context.transform.up * moveY + context.transform.forward * moveZ;
            context.Rb.velocity = movement;

            OnStateExit(context);
        }
    }
    #endregion

    #region REGION - Weapon
    public void OnFireWeaponMelee(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnDefaultFireWeaponMelee();
    }

    public void OnFireWeaponMain(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnDefaultFireWeaponMain();
    }

    public void OnFireWeaponSecondary(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnDefaultFireWeaponSecondary();
    }

    public void OnWeaponChange(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnDefaultWeaponChange();
    }

    public void OnWeaponReload(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnWeaponReload();
    }
    #endregion

    #region REGION - Misc
    public void OnInteract(PlayerContext context)
    {
        // Set Interactable GUI feedback
        RaycastHit hit = context.TryRayCastInteractable();
        context.InteractCanvasHandler.SetActive(hit);

        // EVENT GO HERE
        context.OnDefaultInteract(hit);
    }

    public void OnShowMap(PlayerContext context)
    {
        context.OnDefaultShowMap();
    }
    #endregion

    // SECTION - Method - General =========================================================
    public void OnStateEnter(PlayerContext context) { }

    public void OnStateUpdate(PlayerContext context)
    {
        OnLook(context);
        OnMove(context);
        OnJump(context);

        OnFireWeaponMelee(context);
        OnFireWeaponMain(context);
        OnFireWeaponSecondary(context);
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

        // Airborne
        if (context.Input.Jump || !context.TryRayCastGround().transform)
        {
            return new PlayerStateAirborne();
        }
        else
        {
            if (context.TryRayCastRespawn().transform)
            {
                context.LastSpawnPositionRotation.Position = context.TryRayCastRespawn().transform.position + Vector3.up;
            }
        }

        return this;
    }
}
