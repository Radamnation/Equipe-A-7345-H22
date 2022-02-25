using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // Path Finding

public class Room : MonoBehaviour
{
    // Path Finding 
    private AstarPath myAstarPath;

    // CONFIGURATION
    [SerializeField] private int xDimension = 15;
    [SerializeField] private int zDimension = 15;
    [SerializeField] private int yHeight = 1;
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    [SerializeField] private bool locksOnEnter = true;

    [SerializeField] private bool eastDoorIsBlocked = false;
    [SerializeField] private bool westDoorIsBlocked = false;
    [SerializeField] private bool southDoorIsBlocked = false;
    [SerializeField] private bool northDoorIsBlocked = false;

    [SerializeField] private bool layoutCanBeRotated = false;
    [SerializeField] private bool layoutCanBeMirroredX = false;
    [SerializeField] private bool layoutCanBeMirroredY = false;

    [SerializeField] private bool isCompleted = false;

    [SerializeField] private Transform roomInside;
    [SerializeField] private PositionRotationSO lastSpawnPositionRotation;

    // PREFABS
    [SerializeField] private BiomeInformation biomeInformation;

    private BuildingBlock floorPrefab;
    private BuildingBlock wallPrefab;
    private BuildingBlock ceilingPrefab;
    private DoorBlock doorPrefab;

    private DoorBlock eastDoor;
    private DoorBlock westDoor;
    private DoorBlock southDoor;
    private DoorBlock northDoor;

    private ExitBlock exitBlock;

    public bool EastDoorIsBlocked { get => eastDoorIsBlocked; }
    public bool WestDoorIsBlocked { get => westDoorIsBlocked; }
    public bool SouthDoorIsBlocked { get => southDoorIsBlocked; }
    public bool NorthDoorIsBlocked { get => northDoorIsBlocked; }

    public bool LayoutCanBeRotated { get => layoutCanBeRotated; }
    public bool LayoutCanBeMirroredX { get => layoutCanBeMirroredX; }
    public bool LayoutCanBeMirroredY { get => layoutCanBeMirroredY; }

    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
    public Transform RoomInside { get => roomInside; set => roomInside = value; }
    public AstarPath MyAstarPath { get => myAstarPath; set => myAstarPath = value; }

    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponentInChildren<ExitBlock>() != null)
        {
            exitBlock = GetComponentInChildren<ExitBlock>();
            exitBlock.gameObject.SetActive(false);
        }

        floorPrefab = biomeInformation.floorPrefab;
        wallPrefab = biomeInformation.wallPrefab;
        ceilingPrefab = biomeInformation.ceilingPrefab;
        doorPrefab = biomeInformation.doorPrefab;

        westDoor = Instantiate(doorPrefab, transform);
        westDoor.transform.localPosition = new Vector3(-xDimension / 2, 0, 0);
        westDoor.transform.localRotation = Quaternion.Euler(0, 90, 0);

        eastDoor = Instantiate(doorPrefab, transform);
        eastDoor.transform.localPosition = new Vector3(xDimension / 2, 0, 0);
        eastDoor.transform.localRotation = Quaternion.Euler(0, -90, 0);

        southDoor = Instantiate(doorPrefab, transform);
        southDoor.transform.localPosition = new Vector3(0, 0, -zDimension / 2);
        southDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);

        northDoor = Instantiate(doorPrefab, transform);
        northDoor.transform.localPosition = new Vector3(0, 0, zDimension / 2);
        northDoor.transform.localRotation = Quaternion.Euler(0, 180, 0);

        for (int i = -xDimension / 2; i <= xDimension / 2; i++)
        {
            for (int j = -zDimension / 2; j <= zDimension / 2; j++)
            {
                if (Mathf.Abs(i) == xDimension / 2)
                {
                    if (j != 0 && Mathf.Abs(j) != 1)
                    {
                        // var newFloor = Instantiate(floorPrefab, transform);
                        // newFloor.transform.localPosition = new Vector3(i, -1, j);

                        var newCeiling = Instantiate(ceilingPrefab, transform);
                        newCeiling.transform.localPosition = new Vector3(i, yHeight, j);
                    }
                }
                else if (Mathf.Abs(j) == zDimension / 2)
                {
                    if (i != 0 && Mathf.Abs(i) != 1)
                    {
                        // var newFloor = Instantiate(floorPrefab, transform);
                        // newFloor.transform.localPosition = new Vector3(i, -1, j);

                        var newCeiling = Instantiate(ceilingPrefab, transform);
                        newCeiling.transform.localPosition = new Vector3(i, yHeight, j);
                    }
                }
                else
                {
                    // var newFloor = Instantiate(floorPrefab, transform);
                    // newFloor.transform.localPosition = new Vector3(i, -1, j);

                    var newCeiling = Instantiate(ceilingPrefab, transform);
                    newCeiling.transform.localPosition = new Vector3(i, yHeight, j);
                }
                
                if ((i == -xDimension / 2 || i == xDimension / 2 || j == -zDimension / 2 || j == zDimension / 2) && i != 0 && j != 0 && Mathf.Abs(i) != 1 && Mathf.Abs(j) != 1)
                {
                    var newFloor = Instantiate(floorPrefab, transform);
                    newFloor.transform.localPosition = new Vector3(i, -1, j);

                    var newWall = Instantiate(wallPrefab, transform);
                    newWall.transform.localPosition = new Vector3(i, 0, j);
                    
                    if (yHeight > 1)
                    {
                        newWall = Instantiate(wallPrefab, transform);
                        newWall.transform.localPosition = new Vector3(i, 1, j);
                    }
                }

                if (yHeight > 2 && (i == -xDimension / 2 || i == xDimension / 2 || j == -zDimension / 2 || j == zDimension / 2))
                {
                    for (int k = 2; k <= yHeight; k++)
                    {
                        var newWall = Instantiate(wallPrefab, transform);
                        newWall.transform.localPosition = new Vector3(i, k, j);
                    }
                }
            }
        }
    }

    public void CloseOffWalls(List<Vector3> roomPositions)
    {
        if (!roomPositions.Contains(transform.position / xDimension + new Vector3(1, 0, 0)))
        {
            eastDoor.CloseOffWall();
        }
        if (!roomPositions.Contains(transform.position / xDimension + new Vector3(-1, 0, 0)))
        {
            westDoor.CloseOffWall();
        }
        if (!roomPositions.Contains(transform.position / xDimension + new Vector3(0, 0, 1)))
        {
            northDoor.CloseOffWall();
        }
        if (!roomPositions.Contains(transform.position / xDimension + new Vector3(0, 0, -1)))
        {
            southDoor.CloseOffWall();
        }
    }

    public void OpenAllDoors()
    {
        eastDoor.OpenDoor();
        westDoor.OpenDoor();
        northDoor.OpenDoor();
        southDoor.OpenDoor();
    }

    public void CloseAllDoors()
    {
        eastDoor.CloseDoor();
        westDoor.CloseDoor();
        northDoor.CloseDoor();
        southDoor.CloseDoor();
    }

    public void LockAllDoors()
    {
        eastDoor.IsLocked = true;
        SetIsInteractable(eastDoor, false);

        westDoor.IsLocked = true;
        SetIsInteractable(westDoor, false);

        northDoor.IsLocked = true;
        SetIsInteractable(northDoor, false);

        southDoor.IsLocked = true;
        SetIsInteractable(southDoor, false);
    }

    public void UnlockAllDoors()
    {
        eastDoor.IsLocked = false;
        SetIsInteractable(eastDoor, true);

        westDoor.IsLocked = false;
        SetIsInteractable(westDoor, true);

        northDoor.IsLocked = false;
        SetIsInteractable(northDoor, true);

        southDoor.IsLocked = false;
        SetIsInteractable(southDoor, true);
    }

    public void InitiateRoom()
    {
        if (locksOnEnter)
        {
            LockAllDoors();

            foreach (Room room in mapLayoutInformation.Rooms)
            {
                if (this != room)
                {
                    if (room.IsCompleted)
                    {
                        room.CloseAllDoors();
                        room.LockAllDoors();

                        // Set Path Finding uppon entering new room
                        myAstarPath.data.gridGraph.center = gameObject.transform.localPosition;
                        myAstarPath.data.gridGraph.width = xDimension;
                        myAstarPath.data.gridGraph.depth = zDimension;
                        myAstarPath.Scan();
                    }
                }
            }
        }
        else
        {
            FinishRoom();
        }
    }

    public void FinishRoom()
    {
        Debug.Log($"Testing Living Entities in the room: {gameObject.name}");
        Invoke("TestLivingEntities", 0.1f);
    }

    private void TestLivingEntities()
    {
        var livingEntitiesInsideRoom = GetComponentsInChildren<LivingEntityContext>();
        if (livingEntitiesInsideRoom.Length <= 0)
        {
            IsCompleted = true;
            if (exitBlock != null)
            {
                exitBlock.gameObject.SetActive(true);
            }
            foreach (Room room in mapLayoutInformation.Rooms)
            {
                if (room.IsCompleted)
                {
                    // Interactable sandwish with unlock-all-doors bread
                    room.UnlockAllDoors();

                    SetIsInteractable(eastDoor, false);
                    SetIsInteractable(westDoor, false);
                    SetIsInteractable(northDoor, false);
                    SetIsInteractable(southDoor, false);

                    room.OpenAllDoors();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<LivingEntityContext>() != null)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<LivingEntityContext>().TakeDamage(1);
                RespawnPlayer(other.gameObject);
            }
            else
            {
                other.GetComponent<LivingEntityContext>().TakeDamage(float.MaxValue);
            }
        }
        else
        {
            Destroy(other);
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        // Debug.Log("Applying Spawn Position " + lastSpawnPositionRotation.Transform.position);

        player.transform.position = lastSpawnPositionRotation.Position;
        var playerRigid = player.GetComponent<Rigidbody>();
        playerRigid.velocity = Vector3.zero;
        playerRigid.angularVelocity = Vector3.zero;
    }

    private void SetIsInteractable(DoorBlock doorBlock, bool setTo)
    {
        // Get
        Interactable myDoorInteractable;

        // Set
        myDoorInteractable = doorBlock.GetComponentInChildren<Interactable>();
        if (myDoorInteractable != null) myDoorInteractable.SetIsInteractable(setTo);
    }
}
