using System.Collections.Generic;
using UnityEngine;

public class ShootingRangePedestalArrowsHandler : MonoBehaviour
{
    // SECTION - Field ===================================================================
    //[Header("Arrows")]
    private List<GameObject> myInteractablePanels = new List<GameObject>();


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            myInteractablePanels.Add(transform.GetChild(i).gameObject);
    }


    // SECTION - Method - Utility Specific ===================================================================
    private void OnTriggerEnter(Collider other)
    {
        // Activate pedestal arrows here
        if (other.CompareTag("Player"))
            foreach (GameObject item in myInteractablePanels)
                item.SetActive(true);
    }


    private void OnTriggerExit(Collider other)
    {
        // deactivate pedestal arrows here
        if (other.CompareTag("Player"))
            foreach (GameObject item in myInteractablePanels)
                item.SetActive(false);
    }

}
