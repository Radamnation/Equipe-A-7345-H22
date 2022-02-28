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
        if (room.NorthDoorIsBlocked)
        {
            var tempColor = Color.white;
            tempColor.a = 0;
            northDoorImage.color = tempColor;
        }
        if (room.SouthDoorIsBlocked)
        {
            var tempColor = Color.white;
            tempColor.a = 0;
            southDoorImage.color = tempColor;
        }
        if (room.EastDoorIsBlocked)
        {
            var tempColor = Color.white;
            tempColor.a = 0;
            eastDoorImage.color = tempColor;
        }
        if (room.WestDoorIsBlocked)
        {
            var tempColor = Color.white;
            tempColor.a = 0;
            westDoorImage.color = tempColor;
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
