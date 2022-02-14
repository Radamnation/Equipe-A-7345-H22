using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    // NOTE
    //      - Place inside of EnemyContext?
    //      - Other Solution: Composition ::: EnemyMovement <-O EnemyContext

    // SECTION - Field =========================================================
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform targetTransform;
    private Vector3 destination;


    // SECTION - Method =========================================================
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = targetTransform.position;
        destination = agent.destination;
    }

    private void FixedUpdate()
    {
        // Update destination if the target moves one unit
        if (Vector3.Distance(destination, targetTransform.position) > 0.1f)
        {
            destination = targetTransform.position;
            agent.destination = destination;
            //Vector3.Dot(); // Check if destination = agent.destination; in back or front
        }
    }
}
