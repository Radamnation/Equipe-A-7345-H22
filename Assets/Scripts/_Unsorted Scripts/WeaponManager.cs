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

    private AudioSource weaponAudioSource;

    public WeaponSO Weapon { get => weapon; set => weapon = value; }
    // public WeaponSO SecondaryWeapon { get => secondaryWeapon; set => secondaryWeapon = value; }
    public bool WeaponIsReloading { get => weaponIsReloading; }
    public LayerMask MyTargetMask { get => myTargetMask; }
    public bool TracksPlayer { get => tracksPlayer; set => tracksPlayer = value;  }
    public UnityEvent WeaponHasChanged { get => weaponHasChanged; }
    public UnityEvent WeaponFinishedReloading { get => weaponFinishedReloading; }
    public UnityEvent WeaponHasShot { get => weaponHasShot; }
    public UnityEvent WeaponStartedReloading { get => weaponStartedReloading; }

    // public float SecondaryFireRateDelay { get => secondaryFireRateDelay; set => secondaryFireRateDelay = value; }

    private void Awake()
    {
        weaponAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        fireRateDelay -= Time.deltaTime;
        // secondaryFireRateDelay -= Time.deltaTime;

        // Reload for enemy before they try to shoot | Prevents launching animation when they can't attack
        if (isEnemyWeaponManager && !WeaponIsReloading && weapon.CurrentClip == 0)
        {
            ReloadWeapon();
            weaponIsReloading = true;
        }

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

    public void UpdateSecondaryWeapon() // WeaponSO weapon
    {
        this.weapon = weaponsInventory.EquippedSecondaryWeapon; //weapon;
        //weaponHasChanged.Invoke();
    }

    public void UpdateMeleeWeapon() // WeaponSO weapon
    {
        this.weapon = weaponsInventory.EquippedMeleeWeapon; //weapon;
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
                if (weaponAudioSource != null)
                {
                    weaponAudioSource.PlayOneShot(weapon.ShootingSound[Random.Range(0, weapon.ShootingSound.Length)]);
                }

                StaticDebugger.SimpleDebugger(isDebugOn, $" {weapon.WeaponName} ... FIRED");

                fireRateDelay = weapon.FiringRate;
                validationBool = ShootWeapon(weapon);
                weaponHasShot.Invoke();
                return true;
            }
            else if (!weapon.CanFireContinuously || weapon.CurrentClip == 0)
            {
                if (weaponAudioSource != null && CompareTag("Player"))
                {
                    weaponAudioSource.PlayOneShot(weapon.EmptyClickSound);
                }
                ReloadWeapon();
            }
        }
        else if (fireRateDelay <= 0 && reloadDelay > 0)
        {
            if (weaponAudioSource != null && CompareTag("Player"))
            {
                weaponAudioSource.PlayOneShot(weapon.EmptyClickSound);
            }
            fireRateDelay = weapon.FiringRate;
        }         
        return false;
    }

    public void ReloadWeapon()
    {
        if (!weaponIsReloading && weapon.CurrentClip < weapon.MaxClip)
        {
            if (weapon.ReloadCheck())
            {
                if (weaponAudioSource != null && CompareTag("Player"))
                {
                    weaponAudioSource.PlayOneShot(weapon.ReloadSentenceSound[Random.Range(0, weapon.ReloadSentenceSound.Length)]);
                }

                StaticDebugger.SimpleDebugger(isDebugOn, $" {weapon.WeaponName} ... RELOADED");

                weaponStartedReloading.Invoke();
                reloadDelay = weapon.ReloadTime;

                weaponIsReloading = true;
            }
        }
    }

    public void PlayReloadSound()
    {
        if (weaponAudioSource != null && CompareTag("Player"))
        {
            weaponAudioSource.PlayOneShot(weapon.ReloadSound);
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

        if (transform.parent.CompareTag("Enemy")) // Last minute debug just in case: forces collider instead of trigger
        {
            newProjectile.MyCollider.isTrigger = false;
        }

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

            var spreadDirection = transform.TransformVector(new Vector3(Random.Range(-weapon.Spread, weapon.Spread), Random.Range(-weapon.Spread, weapon.Spread), 0));
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
        hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, Weapon.Range, myTargetMask, isDebugOn);

        //Debug.Log($"Is raycast hit null: {hit.transform == null}");

        return hit.transform != null;
    }

    public bool IsTargetAround()
    {
        Collider[] hit;
        hit = StaticRayCaster.IsOverlapSphereTouching(transform.position, Weapon.Range, myTargetMask, isDebugOn);

        Debug.Log($"Is overlapsphere hit null: {hit == null}");

        return hit != null;
        //return !(hit == null && hit[0].transform != null);
    }
}
