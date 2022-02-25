using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private TransformSO playerTransform;
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    [SerializeField] private MinimapRoom minimapRoomPrefab;
    [SerializeField] private Transform minimapRoomsParent;

    [SerializeField] private float roomScale = 15.0f;
    [SerializeField] private float minimapScale = 5.0f;

    private List<MinimapRoom> minimapRooms = new List<MinimapRoom>();

    // Start is called before the first frame update
    void Start()
    {
        minimapRooms.Clear();
        transform.rotation = Quaternion.Euler(0, 0, 180);
        for (int i = 0; i < mapLayoutInformation.RoomPositions.Count; i++)
        {
            var newMinimapRoom = Instantiate(minimapRoomPrefab);
            newMinimapRoom.transform.parent = minimapRoomsParent;
            var tempPosition = mapLayoutInformation.RoomPositions[i] * minimapScale;
            newMinimapRoom.transform.localPosition = new Vector3(tempPosition.x, tempPosition.z);
            newMinimapRoom.transform.localScale = Vector3.one;
            newMinimapRoom.transform.localRotation = Quaternion.identity;
            newMinimapRoom.PlaceDoors(mapLayoutInformation.Rooms[i]);
            newMinimapRoom.gameObject.SetActive(false);
            minimapRooms.Add(newMinimapRoom);
        }
        UpdateMinimap();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.Transform.rotation.eulerAngles.y));
        var tempPosition = playerTransform.Transform.position / roomScale * minimapScale;
        minimapRoomsParent.localPosition = new Vector3(-tempPosition.x, -tempPosition.z);
    }

    public void UpdateMinimap()
    {
        for (int i = 0; i < minimapRooms.Count; i++)
        {
            if (mapLayoutInformation.Rooms[i].IsVisibleOnMap)
            {
                minimapRooms[i].gameObject.SetActive(true);
                minimapRooms[i].MyImage.color = Color.grey;
            }
            if (mapLayoutInformation.Rooms[i].IsVisitedOnMap)
            {
                minimapRooms[i].gameObject.SetActive(true);
                minimapRooms[i].ShowDoors();
                minimapRooms[i].MyImage.color = Color.white;
            }
        }
    }
}
