using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeArrowUIHandlerWeaponPedestal : AbstractShootingRangeArrowUIHandler
{
    // SECTION - Field ===================================================================
    [SerializeField] private IArrayLinear availableWeaponsSO;


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        SetChildren();

        availableWeaponsSO = GetComponentInParent<ShootingRangeWeaponPedestalManager>().AvailableWeaponsSO;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Activate pedestal arrows here
        if (other.CompareTag(triggerTag) && availableWeaponsSO.Count > 1)
            foreach (GameObject item in myInteractablePanels)
                item.SetActive(true);

        SetToUIVisual(false);
        SetToUIVisual(true);
    }


    private void OnTriggerExit(Collider other)
    {
        // deactivate pedestal arrows here
        if (other.CompareTag(triggerTag))
            foreach (GameObject item in myInteractablePanels)
                item.SetActive(false);
    }


    // SECTION - Method - Utility Specific ===================================================================
    #region REGION - UI Management
    public override void SetToUIVisual(bool isRight)
    {
        if (!isRight)     // <-
        {
            if (availableWeaponsSO.CurrentIndex == 0)                                 // At min index
                toLeft.SetActive(false);
            else if (availableWeaponsSO.CurrentIndex == availableWeaponsSO.Count - 2) // At max index
                toRight.SetActive(true);
        }
        else if (isRight) // ->
        {
            if (availableWeaponsSO.CurrentIndex == availableWeaponsSO.Count - 1)     // At max index
                toRight.SetActive(false);
            else if (availableWeaponsSO.CurrentIndex == 1)                           // At min index
                toLeft.SetActive(true);
        }
    }
    #endregion

}
