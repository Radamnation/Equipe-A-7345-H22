using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRoom : MonoBehaviour
{
    [SerializeField] private Image northDoorImage;
    [SerializeField] private Image southDoorImage;
    [SerializeField] private Image eastDoorImage;
    [SerializeField] private Image westDoorImage;
    [SerializeField] private Image myImage;

    public Image MyImage { get => myImage; set => myImage = value; }

    public void PlaceDoors(Room room)
    {
        foreach (Room adjRoom in room.MyAdjacentRooms)
        {
            if (adjRoom.IsSecretRoom)
            {
                if (room.transform.localPosition - adjRoom.transform.localPosition == new Vector3(0, 0, -room.XDimension))
                {
                    northDoorImage.color = Color.clear;
                }
                if (room.transform.localPosition - adjRoom.transform.localPosition == new Vector3(0, 0, room.XDimension))
                {
                    southDoorImage.color = Color.clear;
                }
                if (room.transform.localPosition - adjRoom.transform.localPosition == new Vector3(-room.XDimension, 0, 0))
                {
                    eastDoorImage.color = Color.clear;
                }
                if (room.transform.localPosition - adjRoom.transform.localPosition == new Vector3(room.XDimension, 0, 0))
                {
                    westDoorImage.color = Color.clear;
                }
            }
        }
        if (room.NorthDoorIsBlocked)
        {
            northDoorImage.color = Color.clear;
        }
        if (room.SouthDoorIsBlocked)
        {
            southDoorImage.color = Color.clear;
        }
        if (room.EastDoorIsBlocked)
        {
            eastDoorImage.color = Color.clear;
        }
        if (room.WestDoorIsBlocked)
        {
            westDoorImage.color = Color.clear;
        }
        northDoorImage.gameObject.SetActive(false);
        southDoorImage.gameObject.SetActive(false);
        eastDoorImage.gameObject.SetActive(false);
        westDoorImage.gameObject.SetActive(false);
    }

    public void ShowDoors()
    {
        northDoorImage.gameObject.SetActive(true);
        southDoorImage.gameObject.SetActive(true);
        eastDoorImage.gameObject.SetActive(true);
        westDoorImage.gameObject.SetActive(true);
    }
}
