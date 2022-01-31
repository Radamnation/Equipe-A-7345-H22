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
        GetComponent<Animator>().SetTrigger("OpenDoor");
    }

    public void CloseDoor()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        OpenDoor();
    }
}
