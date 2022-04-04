using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBlock : MonoBehaviour
{
    [SerializeField] private bool isDoor;
    [SerializeField] private bool isOpen;
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isSecret;

    [SerializeField] private BuildingBlock wallPrefab;
    [SerializeField] private GameObject door;
    [SerializeField] private BoxCollider doorCollider;

    private TorchBlock[] myTorchs;

    public bool IsDoor { get => isDoor; set => isDoor = value; }
    public bool IsOpen { get => isOpen; set => isOpen = value; }
    public bool IsLocked { get => isLocked; set => isLocked = value; }
    public bool IsSecret { get => isSecret; set => isSecret = value; }

    private void Awake()
    {
        myTorchs = GetComponentsInChildren<TorchBlock>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isDoor)
        {
            Instantiate(wallPrefab, transform);
            door.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        if (!isLocked && !isOpen)
        {
            isOpen = true;
            GetComponent<Animator>().SetTrigger("OpenDoor");      
        }
    }

    public void RemoveCollider()
    {
        doorCollider.enabled = false;
    }

    public void InitiateRoom()
    {
        if (!isLocked)
        {
            transform.parent.GetComponent<Room>().InitiateRoom();
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            isOpen = false;
            doorCollider.enabled = true;
            GetComponent<Animator>().SetTrigger("CloseDoor");
        }
    }

    public void CloseOffWall()
    {
        Instantiate(wallPrefab, transform);
        door.SetActive(false);
    }

    public void RemoveTorchs()
    {
        foreach (TorchBlock torch in myTorchs)
        {
            torch.gameObject.SetActive(false);
        }
    }

    public void LitTorchs()
    {
        foreach (TorchBlock torch in myTorchs)
        {
            torch.LitTorch();
        }
    }

    public void UnlitTorchs()
    {
        foreach (TorchBlock torch in myTorchs)
        {
            torch.UnlitTorch();
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    OpenDoor();
    //}
}
