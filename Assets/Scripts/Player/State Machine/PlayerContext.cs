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

    public void OnDefaultMovementBehaviour(float stateDependantModifier = 1.0f)
    {
        float moveX = input.DirX * input.MoveFactor.Value;
        float moveZ = input.DirZ * input.MoveFactor.Value;

        Vector3 movement = (transform.right * moveX +
                            transform.up * rb.velocity.y +
                            transform.forward * moveZ) *
                            stateDependantModifier;

        rb.velocity = movement;
    }

    public void OnDefaultLookBehaviour()
    {
        float lookY = input.LookY * input.MouseSensitivity.Value;

        Vector3 rotationValues = Vector3.up * lookY;

        transform.Rotate(rotationValues);
    }

    public void OnDefaultInteractBehaviour()
    {

    }
}
