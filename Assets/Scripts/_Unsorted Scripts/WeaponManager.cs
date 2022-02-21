using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private bool tracksPlayer = true;
    [SerializeField] private TransformSO playerTransform;
    
    [SerializeField] private WeaponSO mainWeapon;
    [SerializeField] private WeaponSO secondaryWeapon;

    [SerializeField] private UnityEvent mainWeaponFinishedReloading;
    [SerializeField] private UnityEvent mainWeaponHasShot;
    [SerializeField] private UnityEvent mainWeaponStartedReloading;
    [SerializeField] private UnityEvent secondaryWeaponHasShot;

    private float mainFireRateDelay;
    private float mainReloadDelay;
    private float secondaryFireRateDelay;

    private bool mainWeaponIsReloading = false;

    public WeaponSO MainWeapon { get => mainWeapon; set => mainWeapon = value; }
    public WeaponSO SecondaryWeapon { get => secondaryWeapon; set => secondaryWeapon = value; }

    // public float SecondaryFireRateDelay { get => secondaryFireRateDelay; set => secondaryFireRateDelay = value; }
    
    private void Update()
    {
        mainFireRateDelay -= Time.deltaTime;
        secondaryFireRateDelay -= Time.deltaTime;

        if (mainReloadDelay > 0)
        {
            mainReloadDelay -= Time.deltaTime;
            mainWeaponIsReloading = true;
        }
        else if (mainWeaponIsReloading)
        {
            mainWeapon.Reload();
            mainWeaponFinishedReloading.Invoke();
            mainWeaponIsReloading = false;
        }
        
        if (tracksPlayer)
        {
            transform.forward = transform.position - playerTransform.Transform.position;
        }
    }

    public void ResetReload()
    {
        mainReloadDelay = 0;
        mainWeaponIsReloading = false;
    }

    public void TriggerMainWeapon()
    {
        if (mainFireRateDelay <= 0 && mainReloadDelay <= 0)
        {
            if (mainWeapon.ShootCheck())
            {
                Debug.Log($" {mainWeapon.WeaponName} ... FIRED");

                mainFireRateDelay = mainWeapon.FiringRate;
                ShootWeapon(mainWeapon);
                mainWeaponHasShot.Invoke();
            }
            else if (!mainWeapon.CanFireContinuously)
            {
                ReloadMainWeapon();
            }
        }
    }

    public void TriggerSecondaryWeapon()
    {
        if (secondaryFireRateDelay <= 0)
        {
            if (secondaryWeapon.ShootCheck())
            {
                Debug.Log($" {secondaryWeapon.WeaponName} ... FIRED");

                secondaryFireRateDelay = secondaryWeapon.FiringRate;
                ShootWeapon(secondaryWeapon);
                secondaryWeaponHasShot.Invoke();
            }
        }
    }

    public void ReloadMainWeapon()
    {
        if (!mainWeaponIsReloading && mainWeapon.CurrentClip < mainWeapon.MaxClip)
        {
            if (mainWeapon.ReloadCheck())
            {
                Debug.Log($" {mainWeapon.WeaponName} ... RELOADED");
                mainWeaponStartedReloading.Invoke();
                mainReloadDelay = mainWeapon.ReloadTime;
            }
        }
    }

    private void ShootWeapon(WeaponSO weapon)
    {
        if (weapon.Projectile != null)
        {
            ShootProjectile(weapon);
        }
        else
        {
            if (weapon.Spread > 0)
            {
                ShootMultipleRayCasts(weapon);
            }
            else
            {
                ShootSingleRayCast(weapon);
            }
        }
        
    }

    public void ShootProjectile(WeaponSO weapon)
    {
        var newProjectile = Instantiate(weapon.Projectile, transform);
        newProjectile.MyRigidbody.velocity += transform.parent.GetComponent<Rigidbody>().velocity * 0.25f;
        newProjectile.transform.parent = null;
        Debug.Log(newProjectile.name + " was instantiated");
    }

    public void ShootSingleRayCast(WeaponSO weapon)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, GameManager.instance.canBeShotByPlayerMask, 1000);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name + " was hit");
            if (hit.collider.GetComponent<LivingEntityContext>() != null)
            {
                hit.collider.GetComponent<LivingEntityContext>().TakeDamage(weapon.Damage);
            }
            else
            {
                var newBulletHole = Instantiate(weapon.BulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
                newBulletHole.transform.parent = hit.collider.gameObject.transform;
            }
        }
    }

    public void ShootMultipleRayCasts(WeaponSO weapon)
    {
        for (int i = 0; i < weapon.BulletsNumber; i++)
        {
            RaycastHit hit;
            var spreadDirection = new Vector3(0, Random.Range(-weapon.Spread, weapon.Spread), Random.Range(-weapon.Spread, weapon.Spread));
            Physics.Raycast(transform.position, transform.forward + spreadDirection, out hit, GameManager.instance.canBeShotByPlayerMask, 1000);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name + " was hit");
                if (hit.collider.GetComponent<LivingEntityContext>() != null)
                {
                    hit.collider.GetComponent<LivingEntityContext>().TakeDamage(weapon.Damage / weapon.BulletsNumber);
                }
                else
                {
                    var newBulletHole = Instantiate(weapon.BulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
                    newBulletHole.transform.parent = hit.collider.gameObject.transform;
                }
            }
        }
    }
}
