using UnityEngine;
using UnityEngine.Events;

public class EventBasedAttack : AbstractAttackBehaviour
{
    // SECTION - Field ===================================================================
    [Tooltip("Warning: Take good consideration of when and if you want to start an animation as " +
             "there is no way to get a nested conditional validation with Events")]
    [Header("Attack Events")]
    [SerializeField] private UnityEvent myEvents;


    // SECTION - Method ===================================================================
    public override void Execute()
    {
        myEvents.Invoke();
    }

    public override bool IsExecutionValid()
    {
        return base.animConditionBool;
    }
}
