using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private bool isDebugOn = false;

    [Header("Enemy Weapon Manager Section")]
    [SerializeField] private bool isEnemyWeaponManager = true; 
    [SerializeField] private bool tracksPlayer = true;

    [Space(10)]
    [SerializeField] private TransformSO playerTransform;

    [SerializeField] private LayerMask myTargetMask;

    [SerializeField] private WeaponSO weapon;
    // [SerializeField] private WeaponSO secondaryWeapon;
    [SerializeField] private WeaponsInventorySO weaponsInventory;

    [SerializeField] private UnityEvent weaponHasChanged;
    [SerializeField] private UnityEvent weaponFinishedReloading;
    [SerializeField] private UnityEvent weaponHasShot;
    [SerializeField] private UnityEvent weaponStartedReloading;
    // [SerializeField] private UnityEvent secondaryWeaponHasShot;

    private float fireRateDelay;
    private float reloadDelay;
    // private float secondaryFireRateDelay;

    private bool weaponIsReloading = false;

    public WeaponSO Weapon { get => weapon; set => weapon = value; }
    // public WeaponSO SecondaryWeapon { get => secondaryWeapon; set => secondaryWeapon = value; }
    public bool WeaponIsReloading { get => weaponIsReloading; }
    public LayerMask MyTargetMask { get => myTargetMask; }

    // public float SecondaryFireRateDelay { get => secondaryFireRateDelay; set => secondaryFireRateDelay = value; }

    private void Update()
    {
        fireRateDelay -= Time.deltaTime;
        // secondaryFireRateDelay -= Time.deltaTime;

        if (reloadDelay > 0)
        {
            reloadDelay -= Time.deltaTime;
            weaponIsReloading = true;
        }
        else if (weaponIsReloading)
        {
            weapon.Reload();
            weaponFinishedReloading.Invoke();
            weaponIsReloading = false;
        }
        
        if (tracksPlayer)
        {
            transform.forward = playerTransform.Transform.position - transform.position;
        }
    }

    public void UpdateWeapon() // WeaponSO weapon
    {
        this.weapon = weaponsInventory.EquippedMainWeapon; //weapon;
        //weaponHasChanged.Invoke();
    }

    public void ResetReload()
    {
        reloadDelay = 0;
        weaponIsReloading = false;
    }

    public bool TriggerWeapon()
    {
        bool validationBool = false;
        if (fireRateDelay <= 0 && reloadDelay <= 0)
        {
            if (weapon.ShootCheck())
            {
                StaticDebugger.SimpleDebugger(isDebugOn, $" {weapon.WeaponName} ... FIRED");

                fireRateDelay = weapon.FiringRate;
                validationBool = ShootWeapon(weapon);
                weaponHasShot.Invoke();
                return true;
            }
            else if (!weapon.CanFireContinuously || weapon.CurrentClip == 0)
            {
                ReloadWeapon();
            }
        }
        return false;
    }

    public void ReloadWeapon()
    {
        if (!weaponIsReloading && weapon.CurrentClip < weapon.MaxClip)
        {
            if (weapon.ReloadCheck())
            {
                StaticDebugger.SimpleDebugger(isDebugOn, $" {weapon.WeaponName} ... RELOADED");

                weaponStartedReloading.Invoke();
                reloadDelay = weapon.ReloadTime;
            }
        }
    }

    private bool ShootWeapon(WeaponSO weapon)
    {
        bool validationBool = false;

        if (weapon.Projectile != null)
        {
            ShootProjectile(weapon);

            validationBool = true;
        }
        else
        {
            if (weapon.IsMelee)
            {
                validationBool = ShootShortRayCast(weapon);
            }
            else if (weapon.Spread > 0)
            {
                validationBool = ShootMultipleRayCasts(weapon);
            }
            else
            {
                validationBool = ShootSingleRayCast(weapon);
            }
        }

        return validationBool;
    }

    public void ShootProjectile(WeaponSO weapon)
    {
        var newProjectile = Instantiate(weapon.Projectile, transform);
        newProjectile.MyRigidbody.velocity += transform.parent.GetComponent<Rigidbody>().velocity * 0.25f;
        newProjectile.transform.parent = null;

        StaticDebugger.SimpleDebugger(isDebugOn, newProjectile.name + " was instantiated");
    }

    public bool ShootShortRayCast(WeaponSO weapon)
    {
        RaycastHit hit;
        hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, weapon.Range, myTargetMask, isDebugOn);
        if (hit.collider != null)
        {
            StaticDebugger.SimpleDebugger(isDebugOn, hit.collider.name + " was hit");

            if (hit.collider.GetComponent<LivingEntityContext>() != null)
            {
                hit.collider.GetComponent<LivingEntityContext>().TakeDamage(weapon.Damage);
                if (weapon.Knockback > 0)
                {
                    hit.collider.GetComponent<LivingEntityContext>().KnockBack(weapon.Knockback, transform.forward);
                }
                return true;
            }
            //else if (!isEnemyWeaponManager)
            //{
            //    var newBulletHole = Instantiate(weapon.BulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
            //    newBulletHole.transform.parent = hit.collider.gameObject.transform;
            //}
        }
        return false;
    }

    public bool ShootSingleRayCast(WeaponSO weapon)
    {
        RaycastHit hit;
        hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, 1000, myTargetMask, isDebugOn);
        if (hit.collider != null)
        {
            StaticDebugger.SimpleDebugger(isDebugOn, hit.collider.name + " was hit");

            if (hit.collider.GetComponent<LivingEntityContext>() != null)
            {
                hit.collider.GetComponent<LivingEntityContext>().TakeDamage(weapon.Damage);
                return true;
            }
            else if (!isEnemyWeaponManager)
            {
                var newBulletHole = Instantiate(weapon.BulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
                newBulletHole.transform.parent = hit.collider.gameObject.transform;
            }
        }
        return false;
    }

    public bool ShootMultipleRayCasts(WeaponSO weapon)
    {
        bool validationBool = false;

        for (int i = 0; i < weapon.BulletsNumber; i++)
        {
            RaycastHit hit;
            var spreadDirection = new Vector3(0, Random.Range(-weapon.Spread, weapon.Spread), Random.Range(-weapon.Spread, weapon.Spread));
            hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward + spreadDirection, 1000, myTargetMask, isDebugOn);
            if (hit.collider != null)
            {
                StaticDebugger.SimpleDebugger(isDebugOn, hit.collider.name + " was hit");

                if (hit.collider.GetComponent<LivingEntityContext>() != null)
                {
                    hit.collider.GetComponent<LivingEntityContext>().TakeDamage(weapon.Damage / weapon.BulletsNumber);

                    validationBool = true;
                }
                else
                {
                    var newBulletHole = Instantiate(weapon.BulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
                    newBulletHole.transform.parent = hit.collider.gameObject.transform;
                }
            }
        }

        return validationBool;
    }

    public bool IsTargetInFront()
    {
        RaycastHit hit;
        hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, Weapon.Range, myTargetMask, true);

        return hit.transform != null;
    }

    public bool IsTargetAround()
    {
        Collider[] hit;
        hit = StaticRayCaster.IsOverlapSphereTouching(transform, Weapon.Range, myTargetMask, true);

        return !(hit == null && hit[0].transform != null);
    }
}
