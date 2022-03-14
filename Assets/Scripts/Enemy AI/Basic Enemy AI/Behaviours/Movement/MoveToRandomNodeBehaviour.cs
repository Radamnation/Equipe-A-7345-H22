using UnityEngine;
using System.Collections;
using Pathfinding;

public class MoveToRandomNodeBehaviour : AbstractBehaviour
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
        if (myContext.CanUseBehaviour())
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
        if (myContext.MyAIPath.endReachedDistance != 0.32f)
            myContext.SetEndReachedDistance(0.32f);

        if (moveAtSpeed != 0)
            myContext.SetSpeed(moveAtSpeed);
        else
            myContext.SetSpeedAsDefault();
    }

    private void SetNextPosition()
    {
        if (currentMove < maxQuantityOfMove)      // Set new Target when applicable
        {
            myContext.SetMyTemporaryTargetAs(myDesiredRangeOfPositions.GetRandomTransform());

            myContext.SetTarget(myContext.MyTemporaryTargetTransform);
            currentMove++;
        }
        else if (currentMove == maxQuantityOfMove) // Quit behaviour
        {
            // NOTE
            //      -To be changed for desired target
            myContext.SetTarget(GameObject.Find("Player").transform); // myContext.SetTargetAsPlayer() don't work here only???
            //myContext.SetTargetAsPlayer(); // Doesn't work here only???
            myContext.SetEndReachedDistance_ToCurrState();
            myContext.SetSpeedAsDefault();
            currentMove++;
        }
    }

    private IEnumerator StartBehaviour()
    {
        do
        {
            SetNextPosition();
            yield return new WaitForSeconds(1.0f); // Debugger, endReachedDistance always true for attack otherwise
            yield return new WaitUntil(() => myContext.MyAIPath.reachedEndOfPath);
        } while (!myContext.GetTarget().CompareTag("Player")) ;
        // NOTE
        //      -Condition to be changed for desired target

        isExecutionDone = true;
        isValidForExecute = false;
    }
}
