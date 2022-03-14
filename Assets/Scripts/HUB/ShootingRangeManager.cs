using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Shooting Range Variables")]
    [SerializeField] private Transform myCenter;
    [SerializeField] private float rangeX;
    [SerializeField] private float rangeZ;

    [Header("Practice Targets")]
    [SerializeField] private ShootingRangeEnemyListSO myPracticePrefabsSO;
    private List<GameObject> myPracticeTargets;
    private Transform myShootingRangeCenter;


    // SECTION - Property ===================================================================


    // SECTION - Method ===================================================================
    public void StartShootingRange()
    {
        // Clear any leftovers
        myPracticeTargets.Clear();

        float x = myCenter.position.x;
        float z = myCenter.position.z;



        foreach (GameObject practiceTarget in myPracticePrefabsSO.myEnemyList)
        {
            //x = Random.Range();
            //z = Random.Range();

            myPracticeTargets.Add(Instantiate(practiceTarget, GameObject.Find("--------------------- WORLD").transform));
            myPracticeTargets[myPracticeTargets.Count].transform.position = new Vector3(x, 0.0f, z);
        }
    }
}
