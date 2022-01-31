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

    [Header("Rigidbody & Colliders")]
    [SerializeField] LayerMask maskGround;
    [SerializeField] private Rigidbody rb;

    [Header("Scriptable")]
    [SerializeField] private PlayerInputSO input;

    [Header("Animator")]
    [SerializeField] private Animator anim;


    //[Header("Events")]
    // EVENTS GO HERE


    // SECTION - Property ===================================================================
    #region REGION - PROPERTY
    public float DistanceGround { get => distanceGround; }
    public float DistanceInteractible { get => distanceInteractible; }
    public bool IsDebugOn { get => isDebugOn; set => isDebugOn = value; }

    public LayerMask MaskGround { get => maskGround; }
    public Rigidbody Rb { get => rb; set => rb = value; }

    public PlayerInputSO Input { get => input; set => input = value; }

    public Animator Anim { get => anim; set => anim = value; }

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
