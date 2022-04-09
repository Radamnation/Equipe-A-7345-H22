using UnityEngine;
using Pathfinding;
using System.Collections;

public class DontStayIddleBehaviour : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Child Specific")]
    [Tooltip("If false, will move randomly around itself")]
    [SerializeField] private AIPossiblePositionsSO myDesiredRangeOfPositions;
    [SerializeField] private float moveAfterIddleTime = 1.0f;
                     private float currTimer = 0.0f;


    // SECTION - Method - Unity Specific ===================================================================
    public void FixedUpdate()
    {
        if (isPassive)
            Execute();
    }


    // SECTION - Method - Implementation ===================================================================
    public override void Behaviour()
    {
        DontStayIddle();
    }

    public override bool ChildSpecificValidations()
    {
        if (TrySetOverlapSphereHits())
        {
            myContext.SetTargetAsPlayer(); // TEMPORARY - MAKE IT SO IT IS GENERIC-ISH
            return false;
        }

        // Reset Timer
        //if ((myContext.HasToken || myContext.HasPath()) && currTimer != 0.0f)
        if (myContext.HasPath() && currTimer != 0.0f)
            currTimer = 0.0f;

        // Manage timer
        if (myContext.CanUseBehaviour) // Add to timer on iddle 
            currTimer += Time.deltaTime;

        return myContext.IsIddleOrMoving() && currTimer >= moveAfterIddleTime;
    }


    // SECTION - Method - Behaviour Specific ===================================================================
    private void DontStayIddle()
    {
        Debug.Log($"timer On Execute is: {currTimer}");

        // Reset Timer on behaviour use
        currTimer = 0.0f;

        // Set Target
        StartCoroutine(StartMovement());
    }

    private IEnumerator StartMovement()
    {
        for (int i = 0; i < 10; i++)
        {
            AIManager.instance.SetRandomTargetPosition(myContext);

            if (myContext.HasPath())
                break;
        }


        yield return new WaitForSeconds(1.0f); // Debugger, endReachedDistance always true for attack otherwise
        yield return new WaitUntil(() => myContext.MyAIPath.reachedEndOfPath);

        myContext.CanUseBehaviour = true;
        isValidForExecute = false;
    }
}