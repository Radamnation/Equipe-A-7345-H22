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
    [SerializeField] LayerMask maskGround;
    [SerializeField] private Rigidbody rb;

    [Header("Scriptable")]
    [SerializeField] private PlayerInputSO input;

    [Header("Animator")]
    [SerializeField] private Animator anim;


    [Header("Events")]
    [SerializeField] private UnityEvent mainWeaponHasShot;
    [SerializeField] private UnityEvent mainWeaponHasReloaded;
    [SerializeField] private UnityEvent mainWeaponHasChanged;
    [SerializeField] private UnityEvent secondaryWeaponHasShot;

    // SECTION - Property ===================================================================
    #region REGION - PROPERTY
    public float DistanceGround { get => distanceGround; }
    public float DistanceInteractible { get => distanceInteractible; }
    public bool IsDebugOn { get => isDebugOn; set => isDebugOn = value; }

    public WeaponsInventorySO Weapons { get => weapons; set => weapons = value; }
    public WeaponHolder WeaponHolder { get => weaponHolder; set => weaponHolder = value; }
    public TransformSO PlayerTransform { get => playerTransform; set => playerTransform = value; }

    public LayerMask MaskGround { get => maskGround; }
    public Rigidbody Rb { get => rb; set => rb = value; }

    public PlayerInputSO Input { get => input; set => input = value; }

    public Animator Anim { get => anim; set => anim = value; }
    
    public UnityEvent MainWeaponHasShot { get => mainWeaponHasShot; set => mainWeaponHasShot = value; }
    public UnityEvent MainWeaponHasReloaded { get => mainWeaponHasReloaded; set => mainWeaponHasReloaded = value; }
    public UnityEvent MainWeaponHasChanged { get => mainWeaponHasChanged; set => mainWeaponHasChanged = value; }
    public UnityEvent SecondaryWeaponHasShot { get => secondaryWeaponHasShot; set => secondaryWeaponHasShot = value; }

    #endregion


    // SECTION - Method - Unity ===================================================================
    private void Start()
    {
        currState = new PlayerStateGrounded();
        oldState = currState;
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
        return StaticRayCaster.IsTouching(transform.position, -transform.up, DistanceGround, MaskGround, IsDebugOn);
    }
}
