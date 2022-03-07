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
                     private float enemyDamageModifier = 0.25f; // To be changed, bad design. Instantiated projectile would have to have a check for isEnemy and that will become anoying really fast to setup | healths for all should be based on average weapon damage to avoid conditional checks

    [Space(10)]
    [SerializeField] private TransformSO playerTransform;

    [SerializeField] private LayerMask myTargetMask;

    [SerializeField] private WeaponSO weapon;
    // [SerializeField] private WeaponSO secondaryWeapon;
    [SerializeField] private WeaponsInventorySO weaponsInventory;

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

    public void UpdateWeapon()
    {
        weapon = weaponsInventory.EquippedMainWeapon;
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
                weaponHasShot.Invoke(); // if (validationBool) for enemies??
                return true;
            }
            else if (!weapon.CanFireContinuously)
            {
                ReloadWeapon();
            }
        }
        return false;
        //return validationBool;
    }

    //public void TriggerSecondaryWeapon()
    //{
    //    if (secondaryFireRateDelay <= 0)
    //    {
    //        if (secondaryWeapon.ShootCheck())
    //        {
    //            StaticDebugger.SimpleDebugger(isDebugOn, $" {secondaryWeapon.WeaponName} ... FIRED");

    //            secondaryFireRateDelay = secondaryWeapon.FiringRate;
    //            ShootWeapon(secondaryWeapon);
    //            secondaryWeaponHasShot.Invoke();
    //        }
    //    }
    //}

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
        //Physics.Raycast(transform.position, transform.forward, out hit, GameManager.instance.canBeShotByPlayerMask, 1000); // myMask
        hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, weapon.Range, myTargetMask, isDebugOn);
        if (hit.collider != null)
        {
            StaticDebugger.SimpleDebugger(isDebugOn, hit.collider.name + " was hit");

            if (hit.collider.GetComponent<LivingEntityContext>() != null)
            {
                float damage = isEnemyWeaponManager ? weapon.Damage * enemyDamageModifier : weapon.Damage;
                hit.collider.GetComponent<LivingEntityContext>().TakeDamage(damage); // weapon.Damage
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
        //Physics.Raycast(transform.position, transform.forward, out hit, GameManager.instance.canBeShotByPlayerMask, 1000); // myMask
        hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, 1000, myTargetMask, isDebugOn);
        if (hit.collider != null)
        {
            StaticDebugger.SimpleDebugger(isDebugOn, hit.collider.name + " was hit");

            if (hit.collider.GetComponent<LivingEntityContext>() != null)
            {
                float damage = isEnemyWeaponManager ? weapon.Damage * enemyDamageModifier : weapon.Damage;
                hit.collider.GetComponent<LivingEntityContext>().TakeDamage(damage); // weapon.Damage
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
            //Physics.Raycast(transform.position, transform.forward + spreadDirection, out hit, GameManager.instance.canBeShotByPlayerMask, 1000);
            hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward + spreadDirection, 1000, myTargetMask, isDebugOn);
            if (hit.collider != null)
            {
                StaticDebugger.SimpleDebugger(isDebugOn, hit.collider.name + " was hit");

                if (hit.collider.GetComponent<LivingEntityContext>() != null)
                {
                    float damage = isEnemyWeaponManager ? (weapon.Damage * enemyDamageModifier)/ weapon.BulletsNumber : weapon.Damage;
                    hit.collider.GetComponent<LivingEntityContext>().TakeDamage(damage); // weapon.Damage / weapon.BulletsNumber

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

        return !(hit.transform == null);
    }
}
