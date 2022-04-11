using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementWithDestination : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Child Specific")]
    [SerializeField] private AIPossiblePositionsSO myDesiredRangeOfPositions;
    [SerializeField] private int maxQuantityOfMove = 1;
    [SerializeField] private float moveAtSpeed = 0;

    private int currentMove = 0;

    // SECTION - Method - Implementation ===================================================================
    public override void Behaviour()
    {
        OnStartBehaviour();
        StartCoroutine(StartBehaviour());
    }

    public override bool ChildSpecificValidations()
    {
        if (myContext.IsIddleOrMoving())
            return false;

        return true;
    }


    // SECTION - Method - Behaviour Specific ===================================================================
    private void ResetCurrentMoveQuantity()
    {
        currentMove = 0;
    }

    private void OnStartBehaviour()
    {
        ResetCurrentMoveQuantity();

        // Make it so ai must be ON target to reach it
        if (myContext.MyAIPath.endReachedDistance != 0.64f)
            myContext.SetEndReachedDistance(0.64f);

        if (moveAtSpeed != 0)
            myContext.SetSpeed(moveAtSpeed);
        else
            myContext.SetSpeedAsDefault();
    }

    private void SetNextPosition()
    {
        if (currentMove < maxQuantityOfMove)      // Set new Target when applicable
        {
            myDesiredRangeOfPositions.SetRandomTargetPosition(myContext);
            currentMove++;
        }
        else if (currentMove == maxQuantityOfMove) // Quit behaviour
        {
            // NOTE
            //      -To be changed for desired target
            if (myContext.HasToken)
            {
                //myContext.SetTarget(GameObject.Find("Player").transform); // myContext.SetTargetAsPlayer() don't work here only???
                //myContext.SetTargetAsPlayer(); // Doesn't work here only???
                myContext.SetEndReachedDistance_ToCurrState();
            }

            myContext.SetSpeedAsDefault();
            currentMove++;
        }
    }

    private IEnumerator StartBehaviour()
    {
        SetNextPosition();
        myContext.MyAIPath.SearchPath();
        // Wait until we know for sure that the agent has calculated a path to the destination we set above
        while (myContext.MyAIPath.pathPending || !myContext.MyAIPath.reachedEndOfPath)
            yield return null;

        myContext.CanUseBehaviour = true;
        isValidForExecute = false;
    }
}
