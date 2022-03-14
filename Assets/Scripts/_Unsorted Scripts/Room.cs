using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private bool isVisibleOnMap = false;
    [SerializeField] private bool isVisitedOnMap = false;
    [SerializeField] private bool isTreasureRoom = false;
    [SerializeField] private bool isSecretRoom = false;
    [SerializeField] private bool isBossRoom = false;
    [SerializeField] private bool isSpecialRoom = false;
    [SerializeField] private bool isMerchantRoom = false;

    [SerializeField] private Transform roomInside;
    [SerializeField] private PositionRotationSO lastSpawnPositionRotation;

    [SerializeField] private UnityEvent mapHasChanged;

    private List<Room> myAdjacentRooms = new List<Room>();
    private List<LivingEntityContext> myLivingEntities = new List<LivingEntityContext>();

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

    public bool EastDoorIsBlocked { get => eastDoorIsBlocked; set => eastDoorIsBlocked = value; }
    public bool WestDoorIsBlocked { get => westDoorIsBlocked; set => westDoorIsBlocked = value; }
    public bool SouthDoorIsBlocked { get => southDoorIsBlocked; set => southDoorIsBlocked = value; }
    public bool NorthDoorIsBlocked { get => northDoorIsBlocked; set => northDoorIsBlocked = value; }

    public bool LayoutCanBeRotated { get => layoutCanBeRotated; }
    public bool LayoutCanBeMirroredX { get => layoutCanBeMirroredX; }
    public bool LayoutCanBeMirroredY { get => layoutCanBeMirroredY; }

    public int XDimension { get => xDimension; }
    public int ZDimension { get => zDimension; }

    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }
    public bool IsVisibleOnMap { get => isVisibleOnMap; set => isVisibleOnMap = value; }
    public bool IsVisitedOnMap { get => isVisitedOnMap; set => isVisitedOnMap = value; }
    public bool IsTreasureRoom { get => isTreasureRoom; set => isTreasureRoom = value; }
    public bool IsSecretRoom { get => isSecretRoom; set => isSecretRoom = value; }
    public bool IsBossRoom { get => isBossRoom; set => isBossRoom = value; }
    public bool IsSpecialRoom { get => isSpecialRoom; set => isSpecialRoom = value; }
    public bool IsMerchantRoom { get => isMerchantRoom; set => isMerchantRoom = value; }

    public Transform RoomInside { get => roomInside; set => roomInside = value; }
    public AstarPath MyAstarPath { get => myAstarPath; set => myAstarPath = value; }
    
    public List<Room> MyAdjacentRooms { get => myAdjacentRooms; set => myAdjacentRooms = value; }
    public List<LivingEntityContext> MyLivingEntities { get => myLivingEntities; set => myLivingEntities = value; }

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

        var livingEntities = GetComponentsInChildren<LivingEntityContext>();
        foreach (LivingEntityContext livingEntity in livingEntities)
        {
            if (livingEntity.IsEnemy)
            {
                myLivingEntities.Add(livingEntity);
            }
        }
        DeactivateLivingEntities();
    }

    private void DeactivateLivingEntities()
    {
        foreach (LivingEntityContext livingEntity in myLivingEntities)
        {
            livingEntity.gameObject.SetActive(false);
        }
    }

    public void ActivateLivingEntities()
    {
        foreach (LivingEntityContext livingEntity in myLivingEntities)
        {
            livingEntity.gameObject.SetActive(true);
        }
    }

    public void CloseOffWalls(List<Vector3> roomPositions)
    {
        if (!roomPositions.Contains(transform.position / xDimension + new Vector3(1, 0, 0)))
        {
            eastDoor.CloseOffWall();
            eastDoor.RemoveTorchs();
        }
        if (!roomPositions.Contains(transform.position / xDimension + new Vector3(-1, 0, 0)))
        {
            westDoor.CloseOffWall();
            westDoor.RemoveTorchs();
        }
        if (!roomPositions.Contains(transform.position / xDimension + new Vector3(0, 0, 1)))
        {
            northDoor.CloseOffWall();
            northDoor.RemoveTorchs();
        }
        if (!roomPositions.Contains(transform.position / xDimension + new Vector3(0, 0, -1)))
        {
            southDoor.CloseOffWall();
            southDoor.RemoveTorchs();
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
        eastDoor.UnlitTorchs();

        westDoor.IsLocked = true;
        SetIsInteractable(westDoor, false);
        westDoor.UnlitTorchs();

        northDoor.IsLocked = true;
        SetIsInteractable(northDoor, false);
        northDoor.UnlitTorchs();

        southDoor.IsLocked = true;
        SetIsInteractable(southDoor, false);
        southDoor.UnlitTorchs();
    }

    public void UnlockAllDoors()
    {
        eastDoor.IsLocked = false;
        SetIsInteractable(eastDoor, true);
        eastDoor.LitTorchs();

        westDoor.IsLocked = false;
        SetIsInteractable(westDoor, true);
        westDoor.LitTorchs();

        northDoor.IsLocked = false;
        SetIsInteractable(northDoor, true);
        northDoor.LitTorchs();

        southDoor.IsLocked = false;
        SetIsInteractable(southDoor, true);
        southDoor.LitTorchs();
    }

    public void InitiateRoom()
    {
        isVisitedOnMap = true;
        foreach (Room room in MyAdjacentRooms)
        {
            if (!room.isSecretRoom)
            {
                room.isVisibleOnMap = true;
            }
            room.gameObject.SetActive(true);
        }
        mapHasChanged.Invoke();
        if (locksOnEnter && !isCompleted)
        {
            LockAllDoors();
            ActivateLivingEntities();
            foreach (Room room in myAdjacentRooms)
            {
                if (room.IsCompleted)
                {
                    room.CloseAllDoors();
                    room.LockAllDoors();

                    // Set Path Finding uppon entering new room
                    // +1 to dimensions so that enemies can go in between two doors
                    myAstarPath.data.gridGraph.center = gameObject.transform.localPosition;
                    myAstarPath.data.gridGraph.center.y = -1.0f; // Must be at ground level
                    myAstarPath.data.gridGraph.SetDimensions(XDimension * 2 + 2, ZDimension * 2 + 2, myAstarPath.data.gridGraph.nodeSize);
                    myAstarPath.Scan();
                }
            }
            foreach (Room room in mapLayoutInformation.Rooms)
            {
                if (room != this && !myAdjacentRooms.Contains(room))
                {
                    room.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            IsCompleted = true;
            OpenAllDoors();
        }
    }

    public void CheckLivingEntities()
    {
        StaticDebugger.SimpleDebugger(false, $"Testing Living Entities in the room: {gameObject.name}");
        StartCoroutine(StartCheckLivingEntities());
        // Invoke("TestLivingEntities", 0.1f);
    }

    private IEnumerator StartCheckLivingEntities()
    {
        yield return new WaitForEndOfFrame();
        // yield return new WaitForSeconds(0.1f);
        // var livingEntitiesInsideRoom = GetComponentsInChildren<LivingEntityContext>();
        if (myLivingEntities.Count <= 0)
        {
            FinishRoom();
        }
    }

    private void FinishRoom()
    {
        IsCompleted = true;
        if (exitBlock != null)
        {
            exitBlock.gameObject.SetActive(true);
        }
        UnlockAllDoors();
        OpenAllDoors();
        foreach (Room room in myAdjacentRooms)
        {
            if (room.IsCompleted)
            {
                room.UnlockAllDoors();

                SetIsInteractable(eastDoor, false);
                SetIsInteractable(westDoor, false);
                SetIsInteractable(northDoor, false);
                SetIsInteractable(southDoor, false);

                room.OpenAllDoors();
            }
        }
        foreach (Room room in mapLayoutInformation.Rooms)
        {
            if (room.IsVisibleOnMap || room.IsSecretRoom)
            {
                room.gameObject.SetActive(true);
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
                other.GetComponent<LivingEntityContext>().TakeDamage(5);
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
