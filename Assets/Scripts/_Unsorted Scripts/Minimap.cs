using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    //[SerializeField] private Color bossRoomColor;
    //[SerializeField] private Color specialRoomColor;
    //[SerializeField] private Color secretRoomColor;
    //[SerializeField] private Color treasureRoomColor;

    [SerializeField] private TransformSO playerTransform;
    [SerializeField] private MapLayoutInformationSO mapLayoutInformation;
    [SerializeField] private MinimapRoom minimapRoomPrefab;
    [SerializeField] private Transform minimapRoomsParent;

    [SerializeField] private float roomScale = 15.0f;
    [SerializeField] private float minimapScale = 5.0f;

    [SerializeField] private Sprite normalRoomSprite;
    [SerializeField] private Sprite bossRoomSprite;
    [SerializeField] private Sprite treasureRoomSprite;
    [SerializeField] private Sprite secretRoomSprite;
    [SerializeField] private Sprite specialRoomSprite;

    private List<MinimapRoom> minimapRooms = new List<MinimapRoom>();

    // Start is called before the first frame update
    void Start()
    {
        minimapRooms.Clear();
        transform.rotation = Quaternion.Euler(0, 0, 180);
        for (int i = 0; i < mapLayoutInformation.RoomPositions.Count; i++)
        {
            var newMinimapRoom = Instantiate(minimapRoomPrefab);
            newMinimapRoom.transform.SetParent(minimapRoomsParent);
            var tempPosition = mapLayoutInformation.RoomPositions[i] * minimapScale;
            newMinimapRoom.transform.localPosition = new Vector3(tempPosition.x, tempPosition.z);
            newMinimapRoom.transform.localScale = Vector3.one;
            newMinimapRoom.transform.localRotation = Quaternion.identity;
            newMinimapRoom.PlaceDoors(mapLayoutInformation.Rooms[i]);
            if (mapLayoutInformation.Rooms[i].IsBossRoom)
            {
                newMinimapRoom.MyImage.sprite = bossRoomSprite;
            }
            else if (mapLayoutInformation.Rooms[i].IsTreasureRoom)
            {
                newMinimapRoom.MyImage.sprite = treasureRoomSprite;
            }
            else if (mapLayoutInformation.Rooms[i].IsSecretRoom)
            {
                newMinimapRoom.MyImage.sprite = secretRoomSprite;
            }
            else if (mapLayoutInformation.Rooms[i].IsSpecialRoom)
            {
                newMinimapRoom.MyImage.sprite = specialRoomSprite;
            }
            else
            {
                newMinimapRoom.MyImage.sprite = normalRoomSprite;
            }
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
                minimapRooms[i].MyImage.color = new Color(0.25f, 0.25f, 0.25f, 1);
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
