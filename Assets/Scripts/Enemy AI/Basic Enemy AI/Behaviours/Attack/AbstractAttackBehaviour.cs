using UnityEngine;

/// <How_To_Use>
/// 
/// Create a new class which implement this.class
///     - Generate a [IsExecutionValid()] conditionional return for the state machine's animator
///     - Implement [Execute()] as needed
/// 
/// Note
///     - Only the script at the top of the gameObject can be drag & dropped inside of FSM's AbstractAttackBehaviour slots.
///     - It is possible to stack up one or more [AbstractAttackBehaviour.cs] inside of any implemented class
///         + [IsExecutionValid()] can then stack up condition check for the animator and its own [Execute()]
/// 
/// </How_To_Use>

public abstract class AbstractAttackBehaviour : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Tooltip("Ignore if of no use for current behaviour")]
    [Header("Base Class Fields")]
    [SerializeField] private LayerMask tagetMask;
    [SerializeField] protected bool animConditionBool = false;

    // Extend necessary base fields here


    // SECTION - Method ===================================================================
    public abstract bool IsExecutionValid();

    public abstract void Execute();
}
