using UnityEngine;
using System.Collections.Generic;

/// <How_To_Use>
/// 
/// [INSPECTOR]
///     - Target layer mask
///     - endReachedDistance: either from current state's weapon or manual
///     - Validation type : ray, raymultiple, sphere, childbased w.ChildSpecificValidation(), always valid
/// 
/// [Execute()]
///     - Calls the Behaviour() method
///         + Behaviour() to be Implemented in child
///         + IsExecutionValid is called here, if attack is animation based == [BaseEnemyContext.cs] variable
///         
///     - IMPORTANT NOTE
///         + isExecutionDone MUST be reset to true at the end of the Behaviour() call in child
///         
/// [BEHAVIOUR CREATION]
///     - Get & Set for [BaseEnemyContext.cs] from [this.myContext]
///     - Create a new class which implement this.class
///         + Create Desired Behaviour()
///         + [IF NEEDED] - Implement ChildSpecificValidations() to check for validation
/// 
/// </How_To_Use>


public abstract class AbstractBehaviour : MonoBehaviour
{
    // SECTION - Field ===================================================================
    protected BasicEnemyContext myContext;

    protected List<GameObject> myHitsObjs = new List<GameObject>();

    protected readonly float sharedDefaultDistance = 0.64f;

    [Header("Misc")]
    [SerializeField] private bool isDebuggerOn = false;
    [SerializeField] protected LayerMask targetMask;
    [SerializeField] private bool isDistanceCurrWeaponBased;
    [SerializeField] protected float distance;
                     protected bool isValidForExecute = false;
                     protected bool isExecutionDone = true;

    [Header("Type of validation check")]
    [SerializeField] private ValidationCheckTypes validationType = ValidationCheckTypes.ALWAYSVALID;


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        // Get Component
        SetMyBasicEnemyContext();
    }


    // SECTION - Method - System Specific ===================================================================
    public void Execute()
    {
        // [ANIMATION EVENT] - Check Validity
        // Otherwise checked in FSM
        if (myContext.IsAnimExecuteAttack())
            IsExecutionValid();

        // Set Distance
        if (isDistanceCurrWeaponBased && !myContext.IsCurrentWeaponManagerNull() && distance != myContext.GetCurrentWeaponManager().Weapon.Range)
            distance = myContext.GetCurrentWeaponManager().Weapon.Range;

        // Execute
        if (isValidForExecute)
        {
            isExecutionDone = false;
            Behaviour();
            myHitsObjs.Clear();
        }
    }

    public bool IsExecutionValid()
    {
        // If currently executing, not need to process the switch
        if (!isExecutionDone)
            return isExecutionDone;

        switch(validationType)
        {
            case ValidationCheckTypes.CHILDSPECIFIC:            // Set By child
                isValidForExecute = ChildSpecificValidations();
                break;

            case ValidationCheckTypes.ALWAYSVALID:              // Always Valid
                isValidForExecute = true;
                break;

            case ValidationCheckTypes.RAYCASTSINGLE:            // Raycast
                TrySetRaycastSingleHit();
                break;

            case ValidationCheckTypes.RAYCASTARRAY:             // Raycast[]
                TrySetRaycastMultipleHits();
                break;

            case ValidationCheckTypes.OVERLAPSPHERE:            // OverlapSphere
                TrySetOverlapSphereHits();
                break;

        }

        return isValidForExecute && isExecutionDone;
    }

    public abstract void Behaviour();

    public abstract bool ChildSpecificValidations();


    // SECTION - Method - Utility ===================================================================
    protected void SetMyBasicEnemyContext()
    {
        /*
        // context is located in object's parent(base parent) of attack parent(state parent)
        myContext = transform.parent.transform.parent.gameObject.GetComponent<BasicEnemyContext>();

        if (myContext == null)
            myContext = transform.GetComponentInParent<BasicEnemyContext>();
        */

        // context is located in object's parent(base parent) of attack parent(state parent)
        myContext = transform.GetComponentInParent<BasicEnemyContext>();

        if (myContext == null)
            myContext = transform.parent.transform.parent.gameObject.GetComponent<BasicEnemyContext>();
        

    }

    protected void TrySetRaycastSingleHit()
    {
        RaycastHit hit = StaticRayCaster.IsLineCastTouching(myContext.transform.position, myContext.transform.forward, distance, targetMask, isDebuggerOn);

        if (hit.transform)
        {
            myHitsObjs.Add(hit.transform.gameObject);

            isValidForExecute = true;
        }
    }

    protected void TrySetRaycastMultipleHits()
    {
        RaycastHit[] hits = StaticRayCaster.IsLineCastTouchingMultiple(myContext.transform.position, myContext.transform.forward, distance, targetMask, isDebuggerOn);

        if (hits != null)
        {
            foreach (RaycastHit hit in hits)
                myHitsObjs.Add(hit.transform.gameObject);

            isValidForExecute = true;
        }
    }

    protected void TrySetOverlapSphereHits()
    {
        Collider[] hits = StaticRayCaster.IsOverlapSphereTouching(myContext.transform, distance, targetMask, isDebuggerOn);

        if (hits != null)
        {
            foreach(Collider hit in hits)
                myHitsObjs.Add(hit.gameObject);

            isValidForExecute = true;
        }
    }
}
