using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContext : MonoBehaviour
{
    // SECTION - Field ===================================================================
    private IEnemyState currState;
    private IEnemyState oldState;

    [Header("This Living Entity")]
    [SerializeField] private LivingEntityContext myLEC;

    [Header("Target Living Entity")]
    [SerializeField] private LivingEntityContext targetLEC;

    [Header("Raycast")]
    [SerializeField] private float distanceAttack = 0.55f;

    [Header("Animator")]
    [SerializeField] private Animator anim;


    // SECTION - Property ===================================================================
    #region REGION - PROPERTY
    #endregion


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        currState = new EnemyStateRoaming();
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


    // SECTION - Method - State Specific ===================================================================
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


}
