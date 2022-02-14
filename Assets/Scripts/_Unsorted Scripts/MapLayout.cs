using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayout : MonoBehaviour
{
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    [SerializeField] private int minRooms = 10;
    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int roomSize = 15;
    [SerializeField] private int maxGridSize = 3;
    [SerializeField] private Room[] roomPrefabs;

    // Start is called before the first frame update
    void Start()
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

        for (int i = 1; i < numberOfRooms; i++)
        {
            Vector3 startPoint;
            Vector3 offset;
            Vector3 endPoint;
            do
            {
                startPoint = mapLayoutInformation.RoomPositions[Random.Range(0, mapLayoutInformation.RoomPositions.Count - 1)];
                offset = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));
                endPoint = startPoint + offset;
            } while (Mathf.Abs(offset.x) == Mathf.Abs(offset.z) || mapLayoutInformation.RoomPositions.Contains(endPoint));
            
            mapLayoutInformation.RoomPositions.Add(endPoint);
        }
    }

    public void PlaceRooms()
    {
        foreach (Vector3 roomPosition in mapLayoutInformation.RoomPositions)
        {
            var newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length - 1)], roomPosition * roomSize, Quaternion.identity);
            newRoom.transform.parent = transform;
            mapLayoutInformation.Rooms.Add(newRoom);
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
