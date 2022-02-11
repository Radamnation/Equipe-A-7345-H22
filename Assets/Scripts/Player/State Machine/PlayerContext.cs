using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class PlayerContext : MonoBehaviour
{
    // SECTION - Field ===================================================================
    private IPlayerState currState;
    private IPlayerState oldState;

    [Header("Living Entity")]
    [SerializeField] private LivingEntityContext livingEntityContext;

    [Header("Raycast")]
    [SerializeField] private float distanceGround = 0.55f;
    [SerializeField] private float distanceInteractible = 0.75f;
    private bool isDebugOn = false;

    [Header("Weapons")]
    // CURRENT WEAPON & MORE GO HERE
    [SerializeField] private WeaponsInventorySO weapons;
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private TransformSO playerTransform;

    [Header("Rigidbody & Colliders")]
    [SerializeField] private Rigidbody rb;

    [Header("Scriptables")]
    [SerializeField] private PlayerInputSO input;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    [Header("Canvases")]
    [SerializeField] private InteractCanvasHandler interactCanvasHandler;

    [Header("Events")]
    [SerializeField] private UnityEvent mainWeaponHasShot;
    [SerializeField] private UnityEvent mainWeaponHasReloaded;
    [SerializeField] private UnityEvent mainWeaponHasChanged;
    [SerializeField] private UnityEvent secondaryWeaponHasShot;

    // SECTION - Property ===================================================================
    #region REGION - PROPERTY
    public LivingEntityContext LivingEntityContext { get => livingEntityContext; set => livingEntityContext = value; }

    public float DistanceGround { get => distanceGround; }
    public float DistanceInteractible { get => distanceInteractible; }
    public bool IsDebugOn { get => isDebugOn; set => isDebugOn = value; }

    public WeaponsInventorySO Weapons { get => weapons; set => weapons = value; }
    public WeaponHolder WeaponHolder { get => weaponHolder; set => weaponHolder = value; }
    public TransformSO PlayerTransform { get => playerTransform; set => playerTransform = value; }

    public Rigidbody Rb { get => rb; set => rb = value; }

    public PlayerInputSO Input { get => input; set => input = value; }

    public Animator Anim { get => anim; set => anim = value; }
    
    public UnityEvent MainWeaponHasShot { get => mainWeaponHasShot; set => mainWeaponHasShot = value; }
    public UnityEvent MainWeaponHasReloaded { get => mainWeaponHasReloaded; set => mainWeaponHasReloaded = value; }
    public UnityEvent MainWeaponHasChanged { get => mainWeaponHasChanged; set => mainWeaponHasChanged = value; }
    public UnityEvent SecondaryWeaponHasShot { get => secondaryWeaponHasShot; set => secondaryWeaponHasShot = value; }

    public InteractCanvasHandler InteractCanvasHandler { get => interactCanvasHandler; set => interactCanvasHandler = value; }
    #endregion


    // SECTION - Method - Unity ===================================================================
    private void Start()
    {
        currState = new PlayerStateGrounded();
        oldState = currState;

        // TO BE DELETED
        livingEntityContext.FullHeal();
    }

    private void FixedUpdate()
    {
        if (oldState != currState)
        {
            oldState = currState;
            OnStateEnter();
        }

        OnStateUpdate();
        OnStateExit();  
    }


    // SECTION - Method - State Machine ===================================================================
    public void OnStateEnter()
    {
        currState.OnStateEnter(this);
    }

    public void OnStateUpdate()
    {
        currState.OnStateUpdate(this);
    }

    public void OnStateExit()
    {
        currState = currState.OnStateExit(this);
    }


    // SECTION - Method - Utility ===================================================================
    public RaycastHit TryRayCastGround() // Only purpose is to aleviate eye bleeding
    {
        return StaticRayCaster.IsLineCastTouching(transform.position, -transform.up, DistanceGround, GameManager.instance.groundMask, IsDebugOn);
    }

    public RaycastHit TryRayCastInteractable() // Only purpose is to aleviate eye bleeding
    {
        return StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, distanceInteractible, GameManager.instance.interactableMask, isDebugOn);
    }

    #region REGION - Movement
    public void OnDefaultMovement(float stateDependantModifier = 1.0f)
    {
        float moveX = input.DirX * input.MoveFactor.Value;
        float moveZ = input.DirZ * input.MoveFactor.Value;

        Vector3 movement = (transform.right * moveX +
                            transform.up * rb.velocity.y +
                            transform.forward * moveZ) *
                            stateDependantModifier;

        rb.velocity = movement;
    }

    public void OnDefaultLook()
    {
        float lookY = input.LookY * input.MouseSensitivity.Value;

        Vector3 rotationValues = Vector3.up * lookY;

        transform.Rotate(rotationValues);
    }
    #endregion

    #region REGION - Weapon
    public void OnDefaultFireWeaponMain()
    {
        if (input.FireMainWeapon)
        {
            input.FireMainWeapon = false;


            if (weaponHolder.MainFireRateDelay <= 0 && weaponHolder.MainReloadDelay <= 0)
            {
                if (weapons.EquippedMainWeapon.Shoot())
                {
                    Debug.Log($" {weapons.EquippedMainWeapon.WeaponName} ... FIRED");

                    weaponHolder.MainFireRateDelay = weapons.EquippedMainWeapon.FiringRate;
                    if (weapons.EquippedMainWeapon.Projectile != null)
                    {
                        weaponHolder.ShootProjectile(weapons.EquippedMainWeapon.Projectile);
                    }
                    else
                    {
                        if (weapons.EquippedMainWeapon.Spread > 0)
                        {
                            weaponHolder.ShootMultipleRayCasts(10, weapons.EquippedMainWeapon.Spread);
                        }
                        else
                        {
                            weaponHolder.ShootRayCast();
                        }
                    }
                    mainWeaponHasShot.Invoke();
                }
            }
        }
    }

    public void OnDefaultFireWeaponSecondary()
    {
        if (input.FireSecondaryWeapon)
        {
            input.FireSecondaryWeapon = false;

            if (weaponHolder.SecondaryFireRateDelay <= 0)
            {
                if (weapons.EquippedSecondaryWeapon.Shoot())
                {
                    Debug.Log($" {weapons.EquippedSecondaryWeapon.WeaponName} ... FIRED");

                    weaponHolder.SecondaryFireRateDelay = weapons.EquippedSecondaryWeapon.FiringRate;
                    if (weapons.EquippedSecondaryWeapon.Projectile != null)
                    {
                        weaponHolder.ShootProjectile(weapons.EquippedSecondaryWeapon.Projectile);
                    }
                    else
                    {
                        weaponHolder.ShootRayCast();
                    }
                    secondaryWeaponHasShot.Invoke();
                }
            }
        }
    }

    public void OnDefaultWeaponChange()
    {
        if (input.WeaponOne)            // WEAPON ONE
        {
            input.WeaponOne = false;

            // EVENT GO HERE
            weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[0];
            Debug.Log($"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
            mainWeaponHasChanged.Invoke();
        }
        else if (input.WeaponTwo)       // WEAPON TWO
        {
            input.WeaponTwo = false;

            // EVENT GO HERE
            weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[1];
            Debug.Log($"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
            mainWeaponHasChanged.Invoke();
        }
        else if (input.WeaponScrollBackward)       // WEAPON SCROLL <=
        {
            input.WeaponScrollBackward = false;

            // EVENT GO HERE
            var index = weapons.CarriedMainWeapons.IndexOf(weapons.EquippedMainWeapon) - 1;
            if (index < 0)
                index = weapons.CarriedMainWeapons.Count - 1;
            weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[index];
            Debug.Log($"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
            mainWeaponHasChanged.Invoke();
        }
        else if (input.WeaponScrollForward)       // WEAPON SCROLL =>
        {
            input.WeaponScrollForward = false;

            // EVENT GO HERE
            var index = weapons.CarriedMainWeapons.IndexOf(weapons.EquippedMainWeapon) + 1;
            if (index > weapons.CarriedMainWeapons.Count - 1)
                index = 0;
            weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[index];
            Debug.Log($"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
            mainWeaponHasChanged.Invoke();
        }
    }

    public void OnWeaponReload()
    {
        if (input.Reload)
        {
            input.Reload = false;

            // EVENT GO HERE
            if (weaponHolder.MainReloadDelay <= 0)
            {
                if (weapons.EquippedMainWeapon.Reload())
                {
                    Debug.Log($" {weapons.EquippedMainWeapon.WeaponName} ... RELOADED");
                    mainWeaponHasReloaded.Invoke();
                    weaponHolder.MainReloadDelay = weapons.EquippedMainWeapon.ReloadTime;
                }
            }
        }
    }
    #endregion

    #region REGION - Misc
    public void OnDefaultInteract(RaycastHit hit)
    {
        if (input.Interact)
        {
            input.Interact = false;

            if (hit.transform != null)
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();

                // Canvas visual cue
                interactCanvasHandler.SetVisualCue(interactable.IsInteractable);

                // Event launch
                interactable.OnInteraction();
            }
        }
    }

    public void OnDefaultShowMap()
    {
        if (input.ShowMap)
        {
            input.ShowMap = false;

            // EVENT GO HERE
        }
    }
    #endregion
}