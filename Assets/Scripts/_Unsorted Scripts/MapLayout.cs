using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayout : MonoBehaviour
{
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    [SerializeField] private int minRooms = 5;
    [SerializeField] private int maxRooms = 12;
    [SerializeField] private int roomSize = 15;
    [SerializeField] private int bossRooms = 1;
    [SerializeField] private int treasureRooms = 1;
    [SerializeField] private int specialRooms = 2;
    [SerializeField] private int maxGridSize = 3;
    [SerializeField] private RoomsListSO startingRoomsList;
    [SerializeField] private RoomsListSO normalRoomsList;
    [SerializeField] private RoomsListSO bossRoomsList;
    [SerializeField] private RoomsListSO secretRoomsList;
    [SerializeField] private RoomsListSO specialRoomsList;
    [SerializeField] private RoomsListSO treasureRoomsList;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeMapLayout();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeMapLayout()
    {
        GenerateMapLayout();
        PlaceRooms();
        GetNearbyRooms();
        InitializeStartingRoom();
        CloseOffWalls();
        mapLayoutInformation.Rooms[0].OpenAllDoors();
        mapLayoutInformation.Rooms[0].IsCompleted = true;
    }

    public void GenerateMapLayout()
    {
        mapLayoutInformation.RoomPositions.Clear();
        mapLayoutInformation.Rooms.Clear();

        mapLayoutInformation.RoomPositions.Add(new Vector3(0, 0, 0));
        var numberOfRooms = Random.Range(minRooms, maxRooms + 1);

        Vector3 startPoint;
        Vector3 offset;
        Vector3 endPoint;
        for (int i = 1; i < numberOfRooms - bossRooms - treasureRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[Random.Range(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) || mapLayoutInformation.RoomPositions.Contains(endPoint) || CheckRoomsAround(endPoint, 1));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
        for (int i = 0; i < bossRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[Random.Range(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) || mapLayoutInformation.RoomPositions.Contains(endPoint) || CheckRoomsAround(endPoint, 1) || IsTouchingLastXRooms(endPoint, bossRooms));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
        for(int i = 0; i < treasureRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[Random.Range(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) || mapLayoutInformation.RoomPositions.Contains(endPoint) || CheckRoomsAround(endPoint, 1) || IsTouchingLastXRooms(endPoint, bossRooms + treasureRooms));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
        for (int i = 0; i < specialRooms; i++)
        {
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[Random.Range(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) || mapLayoutInformation.RoomPositions.Contains(endPoint) || CheckRoomsAround(endPoint, 1) || IsTouchingLastXRooms(endPoint, bossRooms + treasureRooms + specialRooms));
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
    }

    bool IsTouchingLastXRooms(Vector3 endPoint, int limit)
    {
        for (int i = mapLayoutInformation.RoomPositions.Count - 1; i > mapLayoutInformation.RoomPositions.Count - 1 - limit; i--)
        {
            if (mapLayoutInformation.RoomPositions[i] == endPoint + new Vector3(1, 0, 0)) { return true; };
            if (mapLayoutInformation.RoomPositions[i] == endPoint + new Vector3(-1, 0, 0)) { return true; };
            if (mapLayoutInformation.RoomPositions[i] == endPoint + new Vector3(0, 0, 1)) { return true; };
            if (mapLayoutInformation.RoomPositions[i] == endPoint + new Vector3(0, 0, -1)) { return true; };
        }
        return false;
    }

    bool CheckRoomsAround(Vector3 endPoint, int countLimit)
    {
        var count = 0;
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3(1, 0, 0))) { count++; };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3(-1, 0, 0))) { count++; };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3(0, 0, 1))) { count++; };
        if (mapLayoutInformation.RoomPositions.Contains(endPoint + new Vector3(0, 0, -1))) { count++; };
        if (count > countLimit) { return true; }
        return false;
    }

    public void PlaceRooms()
    {
        AstarPath myAstarPathRef = GameObject.Find("----------------------- TERRAIN").GetComponentInChildren<AstarPath>();

        // Place Starting Room
        var newStartingRoom = Instantiate(startingRoomsList.Rooms[Random.Range(0, startingRoomsList.Rooms.Count)], Vector3.zero, Quaternion.identity);
        newStartingRoom.transform.parent = transform;
        mapLayoutInformation.Rooms.Add(newStartingRoom);
        newStartingRoom.MyAstarPath = myAstarPathRef;

        // Place Normal Rooms
        for (int i = 1; i < mapLayoutInformation.RoomPositions.Count - bossRooms - treasureRooms - specialRooms; i++)
        {
            var newNormalRoom = RotateAndPlaceRoom(normalRoomsList, mapLayoutInformation.RoomPositions[i]);
            // Set AStar reference
            newNormalRoom.MyAstarPath = myAstarPathRef;
        }
        // Place BossRooms
        for (int i = mapLayoutInformation.RoomPositions.Count - bossRooms - treasureRooms - specialRooms; i < mapLayoutInformation.RoomPositions.Count - treasureRooms - specialRooms; i++)
        {
            var newBossRoom = RotateAndPlaceRoom(bossRoomsList, mapLayoutInformation.RoomPositions[i]);
            newBossRoom.IsBossRoom = true;
            newBossRoom.MyAstarPath = myAstarPathRef;
        }
        // Place TreasureRooms
        for (int i = mapLayoutInformation.RoomPositions.Count - treasureRooms - specialRooms; i < mapLayoutInformation.RoomPositions.Count - specialRooms; i++)
        {
            var newTreasureRoom = RotateAndPlaceRoom(treasureRoomsList, mapLayoutInformation.RoomPositions[i]);
            newTreasureRoom.IsTreasureRoom = true;
            newTreasureRoom.MyAstarPath = myAstarPathRef;
        }
        // Place SpecialRooms
        for (int i = mapLayoutInformation.RoomPositions.Count - specialRooms; i < mapLayoutInformation.RoomPositions.Count; i++)
        {
            var newSpecialRoom = RotateAndPlaceRoom(specialRoomsList, mapLayoutInformation.RoomPositions[i]);
            newSpecialRoom.IsSpecialRoom = true;
            newSpecialRoom.MyAstarPath = myAstarPathRef;
        }
    }

    private Room RotateAndPlaceRoom(RoomsListSO roomsList, Vector3 roomPosition)
    {
        var eastHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(roomPosition + new Vector3(1, 0, 0));
        var westHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(roomPosition + new Vector3(-1, 0, 0));
        var northHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(roomPosition + new Vector3(0, 0, 1));
        var southHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(roomPosition + new Vector3(0, 0, -1));

        Room newRoomPrefab;
        float roomRotation;
        bool tempEastHasNoRoom;
        bool tempWestHasNoRoom;
        bool tempNorthHasNoRoom;
        bool tempSouthHasNoRoom;

        do
        {
            newRoomPrefab = roomsList.Rooms[Random.Range(0, roomsList.Rooms.Count)];
            roomRotation = 0;
            tempEastHasNoRoom = eastHasNoRoom;
            tempWestHasNoRoom = westHasNoRoom;
            tempNorthHasNoRoom = northHasNoRoom;
            tempSouthHasNoRoom = southHasNoRoom;

            do
            {
                if ((newRoomPrefab.EastDoorIsBlocked == tempEastHasNoRoom || !newRoomPrefab.EastDoorIsBlocked) &&
                 (newRoomPrefab.WestDoorIsBlocked == tempWestHasNoRoom || !newRoomPrefab.WestDoorIsBlocked) &&
                 (newRoomPrefab.NorthDoorIsBlocked == tempNorthHasNoRoom || !newRoomPrefab.NorthDoorIsBlocked) &&
                 (newRoomPrefab.SouthDoorIsBlocked == tempSouthHasNoRoom || !newRoomPrefab.SouthDoorIsBlocked))
                {
                    break;
                }
                else
                {
                    if (newRoomPrefab.LayoutCanBeRotated)
                    {
                        roomRotation += 90;
                        var temp = tempEastHasNoRoom;
                        tempEastHasNoRoom = tempSouthHasNoRoom;
                        tempSouthHasNoRoom = tempWestHasNoRoom;
                        tempWestHasNoRoom = tempNorthHasNoRoom;
                        tempNorthHasNoRoom = temp;
                    }
                    else
                    {
                        break;
                    }
                }
            } while (roomRotation < 360);
        } while ((newRoomPrefab.EastDoorIsBlocked != tempEastHasNoRoom && newRoomPrefab.EastDoorIsBlocked) ||
                 (newRoomPrefab.WestDoorIsBlocked != tempWestHasNoRoom && newRoomPrefab.WestDoorIsBlocked) ||
                 (newRoomPrefab.NorthDoorIsBlocked != tempNorthHasNoRoom && newRoomPrefab.NorthDoorIsBlocked) ||
                 (newRoomPrefab.SouthDoorIsBlocked != tempSouthHasNoRoom && newRoomPrefab.SouthDoorIsBlocked));

        var newRoom = Instantiate(newRoomPrefab, roomPosition * roomSize, Quaternion.identity);
        
        newRoom.transform.parent = transform;
        newRoom.RoomInside.transform.rotation = Quaternion.Euler(newRoom.transform.rotation.eulerAngles + new Vector3(0, roomRotation, 0));
        while(roomRotation > 0)
        {
            var temp = tempNorthHasNoRoom;
            tempNorthHasNoRoom = tempWestHasNoRoom;
            tempWestHasNoRoom = tempSouthHasNoRoom;
            tempSouthHasNoRoom = tempEastHasNoRoom;
            tempEastHasNoRoom = temp;
            roomRotation -= 90;
        }
        newRoom.EastDoorIsBlocked = tempEastHasNoRoom;
        newRoom.WestDoorIsBlocked = tempWestHasNoRoom;
        newRoom.NorthDoorIsBlocked = tempNorthHasNoRoom;
        newRoom.SouthDoorIsBlocked = tempSouthHasNoRoom;
        
        mapLayoutInformation.Rooms.Add(newRoom);
        newRoom.gameObject.SetActive(false);
        return newRoom;
    }

    private void GetNearbyRooms()
    {
        for (int i = 0; i < mapLayoutInformation.RoomPositions.Count; i++)
        {
            var eastHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(mapLayoutInformation.RoomPositions[i] + new Vector3(1, 0, 0));
            var westHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(mapLayoutInformation.RoomPositions[i] + new Vector3(-1, 0, 0));
            var northHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(mapLayoutInformation.RoomPositions[i] + new Vector3(0, 0, 1));
            var southHasNoRoom = !mapLayoutInformation.RoomPositions.Contains(mapLayoutInformation.RoomPositions[i] + new Vector3(0, 0, -1));

            if (!eastHasNoRoom)
            {
                Room eastRoom = mapLayoutInformation.Rooms[mapLayoutInformation.RoomPositions.IndexOf(mapLayoutInformation.RoomPositions[i] + new Vector3(1, 0, 0))];
                mapLayoutInformation.Rooms[i].MyAdjacentRooms.Add(eastRoom);
            }
            if (!westHasNoRoom)
            {
                Room westRoom = mapLayoutInformation.Rooms[mapLayoutInformation.RoomPositions.IndexOf(mapLayoutInformation.RoomPositions[i] + new Vector3(-1, 0, 0))];
                mapLayoutInformation.Rooms[i].MyAdjacentRooms.Add(westRoom);
            }
            if (!northHasNoRoom)
            {
                Room northRoom = mapLayoutInformation.Rooms[mapLayoutInformation.RoomPositions.IndexOf(mapLayoutInformation.RoomPositions[i] + new Vector3(0, 0, 1))];
                mapLayoutInformation.Rooms[i].MyAdjacentRooms.Add(northRoom);
            }
            if (!southHasNoRoom)
            {
                Room southRoom = mapLayoutInformation.Rooms[mapLayoutInformation.RoomPositions.IndexOf(mapLayoutInformation.RoomPositions[i] + new Vector3(0, 0, -1))];
                mapLayoutInformation.Rooms[i].MyAdjacentRooms.Add(southRoom);
            }
        }
    }

    private void InitializeStartingRoom()
    {
        mapLayoutInformation.Rooms[0].IsVisitedOnMap = true;
        foreach (Room room in mapLayoutInformation.Rooms[0].MyAdjacentRooms)
        {
            room.IsVisibleOnMap = true;
            room.gameObject.SetActive(true);
        }
    }

    private void CloseOffWalls()
    {
        foreach (Room room in mapLayoutInformation.Rooms)
        {
            room.CloseOffWalls(mapLayoutInformation.RoomPositions);
        }
    }
}
