using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pathfinding;

[System.Serializable]
public class ShootingRangeManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Shooting Range Variables")]
    [SerializeField] private Transform shootingRangeCenter;
    [SerializeField] private int rangeX;
    [SerializeField] private int rangeZ;
                     private GameObject[,] myGrid;
                     [SerializeField] private Vector3Int pZero = new Vector3Int();

    [Header("Practice Targets")]
    [SerializeField] private GameObject[] defaultPracticeTargetPrefabs;
    [SerializeField] private ArrayLinearGameObjectSO practiceTargetsSO;
                     //private ArrayLinearGeneric<GameObject> myArrayGeneric_PracticeTarget;
    [SerializeField] private List<GameObject> myPracticeTargetInstances;


    [Header("Weapon Rack")]
    [SerializeField] private WeaponSO[] defaultWeapons;

    private const string myAStarString = "AStar";
    private AstarPath myAstarPath;


    // SECTION - Property ===================================================================


    // SECTION - Method - Unity Specific ===================================================================

    private void Start()
    {
        myGrid = new GameObject[rangeX, rangeZ];

        // Note
        //      - If even number, left side will 1 shorter than right side
        pZero.x = -(rangeX >> 1);
        pZero.z = -(rangeZ >> 1);

        SetAstarPathAndScan();

        SetArrayLinearGenerics();
    }


    // SECTION - Method - Utility Specific ===================================================================
    private void SetArrayLinearGenerics()
    {
        if (practiceTargetsSO.IsEmpty)
            practiceTargetsSO.Copy(defaultPracticeTargetPrefabs);

        //if (defaultPracticeTargetPrefabs != null && myWeaponsSO != null)
            //myWeaponsSO.Copy(defaultWeapons);
    }

    private void SetAstarPathAndScan()
    {
        myAstarPath = GameObject.Find(myAStarString).GetComponent<AstarPath>();

        myAstarPath.data.gridGraph.center = shootingRangeCenter.position;
        myAstarPath.data.gridGraph.center.y = -1.0f; // Must be at ground level

        myAstarPath.data.gridGraph.SetDimensions(rangeX * 2, rangeZ * 2, myAstarPath.data.gridGraph.nodeSize);
        myAstarPath.Scan();
    }

    public void StartRandomPlacementShootingRange()
    {
        // Clear any leftovers
        foreach (GameObject practiceTarget in myPracticeTargetInstances)
            Destroy(practiceTarget);

        myPracticeTargetInstances.Clear();

        if (!practiceTargetsSO.IsEmpty)
            for (int index = 0; index < practiceTargetsSO.Length; index++)
                OnGridInstantiate(practiceTargetsSO.GetElement(index));
    }

    private void OnGridInstantiate(GameObject practiceTarget)
    {
        if (practiceTarget == null)
            return;

        int x = Random.Range(0, rangeX);
        int z = Random.Range(0, rangeZ);

        if (myGrid[x, z] != null)
        {
            OnGridInstantiate(practiceTarget);
            return;
        }

        int posX = pZero.x + x;
        int posZ = pZero.z + z;

        myGrid[x, z] = Instantiate(practiceTarget, shootingRangeCenter.transform);
        myGrid[x, z].transform.localPosition = new Vector3(posX, 0.0f, posZ);
        myGrid[x, z].transform.parent = null;
        
        myPracticeTargetInstances.Add(myGrid[x, z]);
    }
}
