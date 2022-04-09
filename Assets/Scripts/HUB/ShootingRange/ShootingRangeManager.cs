using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

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

    [Header("Preview")]
    [SerializeField] private int quantityPerLine = 6;
    [SerializeField] private GameObject myImagesPrefab;
    [SerializeField] private Transform horizontalLayoutTransform;
                     private List<GameObject> myPreviewList = new List<GameObject>();
                     private float horizontalLayoutOffset = 0.1f;



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

        myAstarPath = AIManager.instance.MyAstarPath;

        // Sprite Renderer
        shootingRange_GroundSprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
        shootingRange_OutlineSprite = transform.GetChild(3).GetComponent<SpriteRenderer>();

        // Animator
        outlineAnimator = shootingRange_OutlineSprite.GetComponent<Animator>();
    }

    private void SetPreviewsImage(GameObject[] myPracticeTargets = null)
    {
        if (myPracticeTargets == null)
            return;

        if (myPreviewList != null && myPreviewList.Count > 0)
            foreach (GameObject obj in myPreviewList)
            {
                Destroy(obj);
            }

        myPreviewList.Clear();

        foreach (GameObject obj in myPracticeTargets)
        {
            myPreviewList.Add(Instantiate(myImagesPrefab, horizontalLayoutTransform));
            myPreviewList[myPreviewList.Count-1].GetComponent<Image>().sprite = obj.GetComponentInChildren<SpriteRenderer>().sprite;

            // Move up to prevent clipping
            if (myPreviewList.Count % quantityPerLine == 0)
            {
                RectTransform myCanvasRect = horizontalLayoutTransform.GetComponent<RectTransform>();

                float x = myCanvasRect.position.x;
                float y = myCanvasRect.position.y + horizontalLayoutOffset;
                float z = myCanvasRect.position.z;


                myCanvasRect.position = new Vector3(x, y, z);
            }         
        }
    }

    private void SetArrayLinearGenerics()
    {
        if (myPracticeTargetPrefabsSO.IsEmpty)
        {
            myPracticeTargetPrefabsSO.Copy(myDefaultPracticeTargetPrefabsSO.GetArray);
        }

        SetPreviewsImage(myPracticeTargetPrefabsSO.GetArray);
    }

    private void SetAstarPathAndScan()
    {
        // Pathfinding
        //myAstarPath = GameObject.Find(myAStarString).GetComponent<AstarPath>();

       // myAstarPath.data.gridGraph.center = shootingRangeCenter.position;
       // myAstarPath.data.gridGraph.center.y = -1.0f; // Must be at ground level

       // myAstarPath.data.gridGraph.SetDimensions((rangeX * 2) + 4, (rangeZ * 2) + 4, myAstarPath.data.gridGraph.nodeSize);

        //myAstarPath.Scan();


        // Set ALL GRIDGRAPHS available to desired settings
        for (int index = 0; index < myAstarPath.data.graphs.Length; index++)
        {
            GridGraph gg = myAstarPath.data.graphs[index] as GridGraph;
            gg.center = shootingRangeCenter.position;
            gg.center.y = -1.0f; // Must be at ground level
            gg.SetDimensions((rangeX * 2) + 4, (rangeZ * 2) + 4, myAstarPath.data.gridGraph.nodeSize);
        }

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
                    for (int index = 0; index < myPracticeTargetPrefabsSO.Count; index++)
                        if (myPracticeTargetPrefabsSO.GetArray[index] != null)
                            OnGridInstantiate(myPracticeTargetPrefabsSO.GetElement(index));
                }
            }
            else // Instantiate defaults
            {
                if (!myPracticeTargetPrefabsSO.IsEmpty)
                {
                    for (int index = 0; index < myDefaultPracticeTargetPrefabsSO.Count; index++)
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
