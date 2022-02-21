using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeLineUpAtkBehaviour : AbstractAttackBehaviour
{
    public override void Execute()
    {
        // What is my purpose?
        // - To do nothing
    }

    public override bool IsExecutionValid()
    {
        return true; // Bullshit return for true testing
    }
}
