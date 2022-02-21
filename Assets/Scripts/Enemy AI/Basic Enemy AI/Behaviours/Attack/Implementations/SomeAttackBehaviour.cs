using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class SomeAttackBehaviour : AbstractAttackBehaviour
{
    [SerializeField] AbstractAttackBehaviour myNextBehaviour;
    [SerializeField] List<AbstractAttackBehaviour> myNextBehaviours;
    [SerializeField] UnityEvent myEventList;

    private void FixedUpdate() // Can call fixed update because base class is monobehaviour! Yay!
    {
        // Some neat behaviour, maybe
    }

    public override void Execute() // From base class
    {
        // Behaviour logic is executed here, or inside another method it calls (i.e. Invoke, IEnumerator, etc.)

        // Lineup of behaviour can be stacked and used
        myNextBehaviour.Execute();
        // Or even 
        foreach(AbstractAttackBehaviour atkBehaviour in myNextBehaviours) atkBehaviour.Execute();
        // Or even
        myEventList.Invoke();

        base.animConditionBool = false;
        // Other resets of value goes here
    }

    public override bool IsExecutionValid() // From base class
    {
        // some random bullshit condition for this.script
        // Also keep track of executability for this.script down the line ifever 
        int a = 1; int b = 2;
        if (a > b) base.animConditionBool = true; 

        // Nested conditional return for lineup of behaviour, if desired
        bool lineUpBool = myNextBehaviour.IsExecutionValid();

        // Raycasts and others can be stacked inside classes and Execute() checks if receipient for them is null
        // receipient and/or animConditionBool management/cleanup for further check may be done manually at end of Execute()
        return lineUpBool || base.animConditionBool; // 1 || 0 == 1 ... ensures that animator launches even if only one is valid
    }
}
