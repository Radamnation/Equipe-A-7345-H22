using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

using Pathfinding;

public abstract class AbstractBehaviour : MonoBehaviour
{
    // SECTION - Field ===================================================================
    protected BasicEnemyContext myContext;

    protected List<GameObject> myHitsObjs = new List<GameObject>();

    protected readonly float sharedDefaultDistance = 0.64f;

    [Header("Misc")]
    [SerializeField] private bool isBypassCanUseBehaviour = false; 
    [Space(10)]
    [SerializeField] protected bool isDebuggerOn = false;
    [Space(10)]
    [SerializeField] protected bool isPassive = false;
    [Space(10)]
    [SerializeField] protected LayerMask targetMask;
    [SerializeField] private bool isDistanceCurrWeaponBased;
    [SerializeField] protected float distance;
                     protected bool isValidForExecute = false;

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
        // [ANIMATION OR PASSIVE]
        //      - Check Validity
        //      - Otherwise checked in FSM
        if (myContext.IsAnimExecuteAttack() || isPassive)
            IsExecutionValid();

        // Set Distance -When Distance is Weapon Based-
        if (isDistanceCurrWeaponBased && !myContext.IsCurrentWeaponManagerNull() && distance != myContext.GetCurrentWeaponManager().Weapon.Range)
            distance = myContext.GetCurrentWeaponManager().Weapon.Range;

        // Execute
        if (isValidForExecute || isBypassCanUseBehaviour)
            StartCoroutine(ExecutionCoroutine());
    }

    private IEnumerator ExecutionCoroutine()
    {
        myContext.CanUseBehaviour = false; // IMPORTANT: Must be set back to true inside of CHILD'S BEHAVIOUR whenever behaviour ends
        Behaviour();
        yield return new WaitUntil(() => myContext.CanUseBehaviour);
        myHitsObjs.Clear();
    }

    public abstract void Behaviour();

    public bool IsExecutionValid()
    {
        // If currently executing, no need to process the switch
        if (!myContext.CanUseBehaviour)
            return myContext.CanUseBehaviour;

        switch (validationType)
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

        return isValidForExecute && myContext.CanUseBehaviour;
    }

    public abstract bool ChildSpecificValidations();


    // SECTION - Method - Utility ===================================================================
    protected void SetMyBasicEnemyContext()
    {
        // context is located in object's parent(base parent) of attack parent(state parent)
        myContext = transform.GetComponentInParent<BasicEnemyContext>();

        if (myContext == null)
            myContext = transform.parent.transform.parent.gameObject.GetComponent<BasicEnemyContext>();
        

    }

    protected bool TrySetRaycastSingleHit()
    {
        RaycastHit hit = StaticRayCaster.IsLineCastTouching(myContext.transform.position, myContext.transform.forward, distance, targetMask, isDebuggerOn);

        if (hit.transform && hit.transform.GetInstanceID() != transform.parent.GetInstanceID())
        {
            myHitsObjs.Add(hit.transform.gameObject);

            isValidForExecute = true;
        }

        return isValidForExecute;
    }

    protected bool TrySetRaycastMultipleHits()
    {
        RaycastHit[] hits = StaticRayCaster.IsLineCastTouchingMultiple(myContext.transform.position, myContext.transform.forward, distance, targetMask, isDebuggerOn);

        if (hits != null && hits[0].transform.GetInstanceID() != transform.parent.GetInstanceID())
        {
            foreach (RaycastHit hit in hits)
                if (hit.transform != null)
                    myHitsObjs.Add(hit.transform.gameObject);

            isValidForExecute = true;
        }

        return isValidForExecute;
    }

    protected bool TrySetOverlapSphereHits()
    {
        Collider[] hits = StaticRayCaster.IsOverlapSphereTouching(myContext.transform, distance, targetMask, isDebuggerOn);

        if (hits != null)
        {
            foreach(Collider hit in hits)
                if(hit.transform != null)
                    myHitsObjs.Add(hit.gameObject);

                isValidForExecute = true;
        }

        return isValidForExecute;
    }
}
