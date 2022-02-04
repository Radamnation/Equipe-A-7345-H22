using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateGrounded : IPlayerState
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

        Vector3 movement = context.transform.right * moveX + context.transform.up * context.Rb.velocity.y + context.transform.forward * moveZ;
        context.Rb.velocity = movement;
    }

    public void OnJump(PlayerContext context)
    {
        if (context.Input.Jump)
        {
            Debug.Log($" {context.name} ... JUMP");

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
    public void OnFireWeaponMain(PlayerContext context)
    {
        if (context.Input.FireMainWeapon)
        {
            Debug.Log($" {context.name} ... FIRE MAIN");


            context.Input.FireMainWeapon = false;

            // EVENT GO HERE
            if (context.WeaponHolder.MainFireRateDelay <= 0 && context.WeaponHolder.MainReloadDelay <= 0)
            {
                if (context.Weapons.EquippedMainWeapon.Shoot())
                {
                    Debug.Log($" {context.Weapons.EquippedMainWeapon.WeaponName} ... FIRED");

                    context.WeaponHolder.MainFireRateDelay = context.Weapons.EquippedMainWeapon.FiringRate;
                    if (context.Weapons.EquippedMainWeapon.Projectile != null)
                    {
                        context.WeaponHolder.ShootProjectile(context.Weapons.EquippedMainWeapon.Projectile);
                    }
                    else
                    {
                        if (context.Weapons.EquippedMainWeapon.Spread > 0)
                        {
                            context.WeaponHolder.ShootMultipleRayCasts(10, context.Weapons.EquippedMainWeapon.Spread);
                        }
                        else
                        {
                            context.WeaponHolder.ShootRayCast();
                        }
                    }
                    context.MainWeaponHasShot.Invoke();
                }
            }
        }
    }

    public void OnFireWeaponOptional(PlayerContext context)
    {
        if (context.Input.FireOptionalWeapon)
        {
            Debug.Log($" {context.name} ... FIRE OPTIONAL");


            context.Input.FireOptionalWeapon = false;

            // EVENT GO HERE
            if (context.WeaponHolder.SecondaryFireRateDelay <= 0)
            {
                if (context.Weapons.EquippedSecondaryWeapon.Shoot())
                {
                    Debug.Log($" {context.Weapons.EquippedSecondaryWeapon.WeaponName} ... FIRED");

                    context.WeaponHolder.SecondaryFireRateDelay = context.Weapons.EquippedSecondaryWeapon.FiringRate;
                    if (context.Weapons.EquippedSecondaryWeapon.Projectile != null)
                    {
                        context.WeaponHolder.ShootProjectile(context.Weapons.EquippedSecondaryWeapon.Projectile);
                    }
                    else
                    {
                        context.WeaponHolder.ShootRayCast();
                    }
                    context.SecondaryWeaponHasShot.Invoke();
                }
            }
        }
    }

    public void OnWeaponChange(PlayerContext context)
    {
        if (context.Input.WeaponOne)            // WEAPON ONE
        {
            Debug.Log($" {context.name} ... CHANGE WEAPON ONE");


            context.Input.WeaponOne = false;

            // EVENT GO HERE
            context.Weapons.EquippedMainWeapon = context.Weapons.CarriedMainWeapons[0];
            Debug.Log($"MAIN WEAPON CHANGED TO ... {context.Weapons.EquippedMainWeapon.WeaponName}");
            context.MainWeaponHasChanged.Invoke();
        }
        else if (context.Input.WeaponTwo)       // WEAPON TWO
        {
            Debug.Log($" {context.name} ... CHANGE WEAPON TWO");


            context.Input.WeaponTwo = false;

            // EVENT GO HERE
            context.Weapons.EquippedMainWeapon = context.Weapons.CarriedMainWeapons[1];
            Debug.Log($"MAIN WEAPON CHANGED TO ... {context.Weapons.EquippedMainWeapon.WeaponName}");
            context.MainWeaponHasChanged.Invoke();
        }
        else if (context.Input.WeaponScrollBackward)       // WEAPON SCROLL <=
        {
            Debug.Log($" {context.name} ... CHANGE WEAPON SCROLL <=");


            context.Input.WeaponScrollBackward = false;

            // EVENT GO HERE
            var index = context.Weapons.CarriedMainWeapons.IndexOf(context.Weapons.EquippedMainWeapon) - 1;
            if (index < 0)
                index = context.Weapons.CarriedMainWeapons.Count - 1;
            context.Weapons.EquippedMainWeapon = context.Weapons.CarriedMainWeapons[index];
            Debug.Log($"MAIN WEAPON CHANGED TO ... {context.Weapons.EquippedMainWeapon.WeaponName}");
            context.MainWeaponHasChanged.Invoke();
        }
        else if (context.Input.WeaponScrollForward)       // WEAPON SCROLL =>
        {
            Debug.Log($" {context.name} ... CHANGE WEAPON SCROLL =>");


            context.Input.WeaponScrollForward = false;

            // EVENT GO HERE
            var index = context.Weapons.CarriedMainWeapons.IndexOf(context.Weapons.EquippedMainWeapon) + 1;
            if (index > context.Weapons.CarriedMainWeapons.Count - 1)
                index = 0;
            context.Weapons.EquippedMainWeapon = context.Weapons.CarriedMainWeapons[index];
            Debug.Log($"MAIN WEAPON CHANGED TO ... {context.Weapons.EquippedMainWeapon.WeaponName}");
            context.MainWeaponHasChanged.Invoke();
        }
    }

    public void OnWeaponReload(PlayerContext context)
    {
        if (context.Input.Reload)
        {
            Debug.Log($" {context.name} ... RELOAD");


            context.Input.Reload = false;

            // EVENT GO HERE
            if (context.WeaponHolder.MainReloadDelay <= 0)
            {
                if (context.Weapons.EquippedMainWeapon.Reload())
                {
                    Debug.Log($" {context.Weapons.EquippedMainWeapon.WeaponName} ... RELOADED");
                    context.MainWeaponHasReloaded.Invoke();
                    context.WeaponHolder.MainReloadDelay = context.Weapons.EquippedMainWeapon.ReloadTime;
                }
            }
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
    public void OnStateEnter(PlayerContext context) { }

    public void OnStateUpdate(PlayerContext context)
    {
        OnLook(context);
        OnMove(context);
        OnJump(context);
        
        // Added to have player position known to everything
        context.PlayerTransform.Transform = context.transform;


        OnFireWeaponMain(context);
        OnFireWeaponOptional(context);
        OnWeaponChange(context);
        OnWeaponReload(context);

        OnInteract(context);
        OnShowMap(context);
    }

    public IPlayerState OnStateExit(PlayerContext context)
    {
        // Airborne
        if (context.Input.Jump)
            if (!context.TryRayCastGround().transform)               
                return new PlayerStateAirborne();

        return this;
    }
}
