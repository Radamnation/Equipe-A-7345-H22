using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootingRangeManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Shooting Range Variables")]
    [SerializeField] private int rangeX;
    [SerializeField] private int rangeY;
    [SerializeField] private int rangeZ;
                     private bool isPlayerPresent = false;
                     private Transform shootingRangeCenter;
                     private BoxCollider shootingRangeCenterCollider;

                     private GameObject[,] myGrid;
                     private Vector3Int pZero = new Vector3Int();

    [Header("Practice Targets")]
    [SerializeField] private ArrayLinearGameObjectSO myDefaultPracticeTargetPrefabsSO;
    [SerializeField] private ArrayLinearGameObjectSO myPracticeTargetPrefabsSO;
    [SerializeField] private List<GameObject> myPracticeTargetInstances = new List<GameObject>();

                     private const string myAStarString = "AStar";
                     private AstarPath myAstarPath; // Width and Depth are : (grid.N * 2) + 4

    //[Header("Shooting Range Ground")]
    // Note:
    //      - Tiled : X & Y :  0.32 * ((grid.N + 1) * 2)
                     private Animator outlineAnimator;
                     private const string outlineAnimString = "isShootingRangeActive";
                     private SpriteRenderer shootingRange_GroundSprite;
                     private SpriteRenderer shootingRange_OutlineSprite;
                     private const float spritesY = -0.49f;


    // SECTION - Property ===================================================================


    // SECTION - Method - Unity Specific ===================================================================

    private void Start()
    {
        GetComponentsAtStart();

        // Note
        //      - If even number, left side will be 1 square shorter than right side
        pZero.x = -(rangeX >> 1);
        pZero.z = -(rangeZ >> 1);

        SetAstarPathAndScan();

        InstantiateShootingRange(true); // Instantiate default set of practice targets

        SetArrayLinearGenerics();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerPresent = true;

            if (myPracticeTargetInstances.Count != 0)
            {
                outlineAnimator.SetBool(outlineAnimString, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset shooting range if player gets outisde sumo ring
        //      - Prevents abuse by shooting from outside enemies' range
        if (other.CompareTag("Player"))
        {
            isPlayerPresent = false;

            outlineAnimator.SetBool(outlineAnimString, false);

            // Prevents first spawn of practice targets when moving around empty shooting range
            if (myPracticeTargetInstances.Count != 0)
                InstantiateShootingRange(true);
        }
        else if (other.gameObject.layer == 8) // Layer int # for LIVING ENTITY
        {
            other.transform.position = shootingRangeCenter.position;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    // SECTION - Method - Utility Specific ===================================================================
    private void GetComponentsAtStart()
    {
        // Shooting Range Center
        shootingRangeCenter = transform.GetChild(1).transform;
        myGrid = new GameObject[rangeX, rangeZ];
        shootingRangeCenterCollider = shootingRangeCenter.GetComponent<BoxCollider>();
        shootingRangeCenterCollider.size = new Vector3(rangeX, rangeY, rangeZ);

        myAstarPath = GameObject.Find(myAStarString).GetComponent<AstarPath>();

        // Sprite Renderer
        shootingRange_GroundSprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
        shootingRange_OutlineSprite = transform.GetChild(3).GetComponent<SpriteRenderer>();

        // Animator
        outlineAnimator = shootingRange_OutlineSprite.GetComponent<Animator>();
    }

    private void SetArrayLinearGenerics()
    {
        if (myPracticeTargetPrefabsSO.IsEmpty)
            myPracticeTargetPrefabsSO.Copy(myDefaultPracticeTargetPrefabsSO.GetArray);
    }

    private void SetAstarPathAndScan()
    {
        // Pathfinding
        //myAstarPath = GameObject.Find(myAStarString).GetComponent<AstarPath>();

        myAstarPath.data.gridGraph.center = shootingRangeCenter.position;
        myAstarPath.data.gridGraph.center.y = -1.0f; // Must be at ground level

        myAstarPath.data.gridGraph.SetDimensions((rangeX * 2) + 4, (rangeZ * 2) + 4, myAstarPath.data.gridGraph.nodeSize);

        myAstarPath.Scan();

        // Shooting range's ground
        float x = shootingRangeCenter.position.x;
        float z = shootingRangeCenter.position.z;
        Vector3 spritesPos = new Vector3(x, spritesY, z);

        shootingRange_GroundSprite.transform.position = spritesPos;
        shootingRange_OutlineSprite.transform.position = spritesPos;

        shootingRange_GroundSprite.size = new Vector2((myAstarPath.data.gridGraph.nodeSize * myAstarPath.data.gridGraph.width) - 1, (myAstarPath.data.gridGraph.nodeSize * myAstarPath.data.gridGraph.depth) - 1);
        shootingRange_OutlineSprite.size = new Vector2((myAstarPath.data.gridGraph.nodeSize * myAstarPath.data.gridGraph.width) - 1, (myAstarPath.data.gridGraph.nodeSize * myAstarPath.data.gridGraph.depth) - 1);
    }

    public void InstantiateShootingRange(bool instantiateDefaults = false)
    {
        if (!isPlayerPresent) // || myPracticeTargetInstances.Count == 0
        {
            // Clear any leftovers
            foreach (GameObject practiceTarget in myPracticeTargetInstances)
                Destroy(practiceTarget);

            myPracticeTargetInstances.Clear();

            if (!instantiateDefaults) // Instantiate prefabs
            {
                if (!myPracticeTargetPrefabsSO.IsEmpty)
                {
                    for (int index = 0; index < myPracticeTargetPrefabsSO.Length; index++)
                        if (myPracticeTargetPrefabsSO.GetArray[index] != null)
                            OnGridInstantiate(myPracticeTargetPrefabsSO.GetElement(index));
                }
            }
            else // Instantiate defaults
            {
                if (!myPracticeTargetPrefabsSO.IsEmpty)
                {
                    for (int index = 0; index < myDefaultPracticeTargetPrefabsSO.Length; index++)
                        if (myDefaultPracticeTargetPrefabsSO.GetArray[index] != null)
                            OnGridInstantiate(myDefaultPracticeTargetPrefabsSO.GetElement(index));
                }
            }
        }
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
