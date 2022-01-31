using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // CONFIGURATION
    [SerializeField] private int xDimension = 15;
    [SerializeField] private int zDimension = 15;
    [SerializeField] private int yHeight = 1;

    // PREFABS
    [SerializeField] private BiomeInformation biomeInformation;
    private BuildingBlock floorPrefab;
    private BuildingBlock wallPrefab;
    private BuildingBlock ceilingPrefab;
    private DoorBlock doorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        floorPrefab = biomeInformation.floorPrefab;
        wallPrefab = biomeInformation.wallPrefab;
        ceilingPrefab = biomeInformation.ceilingPrefab;
        doorPrefab = biomeInformation.doorPrefab;

        var eastDoor = Instantiate(doorPrefab, transform);
        eastDoor.transform.localPosition = new Vector3(-xDimension / 2, 0, 0);
        eastDoor.transform.localRotation = Quaternion.Euler(0, 90, 0);
        var westDoor = Instantiate(doorPrefab, transform);
        westDoor.transform.localPosition = new Vector3(xDimension / 2, 0, 0);
        westDoor.transform.localRotation = Quaternion.Euler(0, -90, 0);
        var southDoor = Instantiate(doorPrefab, transform);
        southDoor.transform.localPosition = new Vector3(0, 0, -zDimension / 2);
        southDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
        var northDoor = Instantiate(doorPrefab, transform);
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

                if ((i == -xDimension / 2 || i == xDimension / 2 || j == -zDimension / 2 ||  j == zDimension / 2) && i != 0 && j != 0 && Mathf.Abs(i) != 1 && Mathf.Abs(j) != 1)
                {
                    var newWall = Instantiate(wallPrefab, transform);
                    newWall.transform.localPosition = new Vector3(i, 0, j);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
