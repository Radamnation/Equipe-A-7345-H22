using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // CONFIGURATION
    [SerializeField] private int xDimension = 15;
    [SerializeField] private int zDimension = 15;
    [SerializeField] private int yHeight = 1;
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;

    [SerializeField] private bool isCompleted = false;

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

    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }

    // Start is called before the first frame update
    void Awake()
    {
        yHeight--; // TO BE DELETED

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
                        var newFloor = Instantiate(floorPrefab, transform);
                        newFloor.transform.localPosition = new Vector3(i, -1, j);

                        var newCeiling = Instantiate(ceilingPrefab, transform);
                        newCeiling.transform.localPosition = new Vector3(i, 1, j);
                    }
                }
                else if (Mathf.Abs(j) == zDimension / 2)
                {
                    if (i != 0 && Mathf.Abs(i) != 1)
                    {
                        var newFloor = Instantiate(floorPrefab, transform);
                        newFloor.transform.localPosition = new Vector3(i, -1, j);

                        var newCeiling = Instantiate(ceilingPrefab, transform);
                        newCeiling.transform.localPosition = new Vector3(i, 1, j);
                    }
                }
                else
                {
                    var newFloor = Instantiate(floorPrefab, transform);
                    newFloor.transform.localPosition = new Vector3(i, -1, j);

                    var newCeiling = Instantiate(ceilingPrefab, transform);
                    newCeiling.transform.localPosition = new Vector3(i, 1, j);
                }

                if ((i == -xDimension / 2 || i == xDimension / 2 || j == -zDimension / 2 || j == zDimension / 2) && i != 0 && j != 0 && Mathf.Abs(i) != 1 && Mathf.Abs(j) != 1)
                {
                    var newWall = Instantiate(wallPrefab, transform);
                    newWall.transform.localPosition = new Vector3(i, 0, j);
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
        westDoor.IsLocked = true;
        northDoor.IsLocked = true;
        southDoor.IsLocked = true;
    }

    public void UnlockAllDoors()
    {
        eastDoor.IsLocked = false;
        westDoor.IsLocked = false;
        northDoor.IsLocked = false;
        southDoor.IsLocked = false;
    }

    public void InitiateRoom()
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
                }
            }
        }
    }

    public void FinishRoom()
    {
        Debug.Log("Testing Living Entities in the room");
        Invoke("TestLivingEntities", 0.1f);
    }

    private void TestLivingEntities()
    {
        var livingEntitiesInsideRoom = GetComponentsInChildren<LivingEntityContext>();
        if (livingEntitiesInsideRoom.Length <= 0)
        {
            IsCompleted = true;
            foreach (Room room in mapLayoutInformation.Rooms)
            {
                if (room.IsCompleted)
                {
                    room.UnlockAllDoors();
                    room.OpenAllDoors();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
