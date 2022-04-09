using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateAfterIterations : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Child Specific")]
    [SerializeField] private BasicEnemy_States myDesiredState = BasicEnemy_States.ONE;
    [Space(10)]
    [SerializeField] private int count = 0;
    [SerializeField] private int desiredCount = 5;



    // SECTION - Method - Implementation ===================================================================
    public override void Behaviour()
    {
        StartBehaviour();
    }

    public override bool ChildSpecificValidations()
    {
        return true;
    }


    // SECTION - Method - Behaviour Specific ===================================================================
    private void StartBehaviour()
    {
        myContext.CanUseBehaviour = false;

        count++;

        if (count >= desiredCount)
        {
            myContext.SetFiniteStateMachine(myDesiredState);
            count = 0;
        }

        myContext.CanUseBehaviour = true;
    }
}
