using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/Weapon", fileName = "WeaponSO")]
public class WeaponSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    [Header("Information")]
    [SerializeField] private string weaponName;
    [SerializeField] private string weaponDescription;

    [Header("Value")]
    [SerializeField] private int currencyValue;

    [Header("Statistics")]
    [SerializeField] private bool canFireContinuously;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int currentClip;
    [SerializeField] private int maxClip;
    [SerializeField] private float firingRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private float damage;
    [SerializeField] private float spread;
    [SerializeField] private float range;
    [SerializeField] private int bulletsNumber;
    [SerializeField] private PhysicalProjectile projectile;

    [Header("Visual")]
    [SerializeField] private BulletHole bulletHole;
    [SerializeField] private Sprite weaponUISprite;
    [SerializeField] private Sprite weaponPlayerSprite;
    [SerializeField] private Animator animator;

    // SECTION - Property ===================================================================
    public string WeaponName { get => weaponName; set => weaponName = value; }
    public string WeaponDescription { get => weaponDescription; set => weaponDescription = value; }
    
    public int CurrencyValue { get => currencyValue; set => currencyValue = value; }

    public bool CanFireContinuously { get => canFireContinuously; set => canFireContinuously = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }
    public int CurrentClip { get => currentClip; set => currentClip = value; }
    public int MaxClip { get => maxClip; set => maxClip = value; }
    public float FiringRate { get => firingRate; set => firingRate = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Spread { get => spread; set => spread = value; }
    public float Range { get => range; set => range = value; }
    public int BulletsNumber { get => bulletsNumber; set => bulletsNumber = value; }

    public PhysicalProjectile Projectile { get => projectile; set => projectile = value; }
    public BulletHole BulletHole { get => bulletHole; set => bulletHole = value; }
    public Sprite WeaponUISprite { get => weaponUISprite; set => weaponUISprite = value; }
    public Sprite WeaponPlayerSprite { get => weaponPlayerSprite; set => weaponPlayerSprite = value; }
    public Animator Animator { get => animator; set => animator = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        currentAmmo = maxAmmo;
        currentClip = maxClip;
    }

    public bool ShootCheck()
    {
        if (currentClip > 0)
        {
            currentClip--;
            return true;
        }
        return false;
    }

    public bool ReloadCheck()
    {
        if (currentAmmo > 0)
        {
            return true;
        }
        return false;
    }

    public void Reload()
    {
        if (currentAmmo >= maxClip - currentClip)
        {
            currentAmmo -= (maxClip - currentClip);
            currentClip = maxClip;
        }
        else
        {
            currentClip += currentAmmo;
            currentAmmo = 0;
        }
    }
}
