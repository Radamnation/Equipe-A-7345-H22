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
    [Tooltip("Ignore if of no use for current behaviour")]
    [Header("Base Class Fields")]
    [SerializeField] private Transform myMovementTarget;

    // Extend necessary base fields here


    // SECTION - Method ============================================================
    public abstract void Execute();
}
