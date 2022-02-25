using UnityEngine;

/// <How_To_Use>
/// 
/// Create a new class which implement this.class
///     - Implement [Execute()] as needed
/// 
/// Note
///     - Only the script at the top of the gameObject can be drag & dropped inside of FSM's AbstractAttackBehaviour slots.
///     - It is possible to stack up one or more [AbstractMovementBehaviour.cs] inside of any implemented class
///         
/// </How_To_Use>


public abstract class AbstractMovementBehaviour : MonoBehaviour
{
    // SECTION - Field ============================================================
    private BasicEnemyContext myContext;

    [Tooltip("Ignore if of no use for current behaviour")]
    [Header("Base Class Fields")]
    [SerializeField] private GameObject myTargetPrefab;
    [SerializeField] private Transform myMovementTarget;
 

    // Extend necessary base fields here


    // SECTION - Property ===================================================================
    public BasicEnemyContext MyContext { get => myContext; }


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        // Get Component
        // context is located in object's parent(base parent) of attack parent(state parent)
        myContext = transform.parent.transform.parent.gameObject.GetComponent<BasicEnemyContext>();

        if (myContext == null)
            myContext = transform.GetComponentInParent<BasicEnemyContext>();
    }


    // SECTION - Method - Abstract Specific ============================================================
    public abstract void Execute();
}
