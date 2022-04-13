using System.Collections.Generic;
using UnityEngine;

/// <NOTE>
/// 
/// This script suffers from a lack of generic of GenericArrayLinear
///     - TODO:
///         + Create a true generic array linear in the form of a SCRIPTABLE OBJECT -unity hates it, but find a way-
///         + Merge this.script && ShootingRangeArrowUIHandlerBell.cs into one 
///         + Correct ShootingRangeManager && this.script as necessary to implement one getcomponent only...
///             ... or -if generic allows it- plug proper generic array linear into this.script
///             ++ Getcomponent<MyDelegateScriptToScriptable>().StoredReference; could be used -not the best-
///             
/// </NOTE>

public class ShootingRangeArrowUIHandlerWeaponPedestal : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Manage If Tag")]
    [SerializeField] protected string triggerTag = "Player";

    [Header("To X UI Elements")]
    [SerializeField] protected GameObject toLeft;
    [SerializeField] protected GameObject toRight;

    protected List<GameObject> myInteractablePanels = new List<GameObject>();
    protected IArrayLinear availableItemsSO;


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        SetChildren();

        availableItemsSO = GetComponentInParent<ShootingRangeWeaponPedestalManager>().AvailableWeaponsSO;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Activate pedestal arrows here
        if (other.CompareTag(triggerTag) && availableItemsSO.Count > 1)
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
    protected void SetChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
            myInteractablePanels.Add(transform.GetChild(i).gameObject);
    }

    public void SetToUIVisual(bool isRight)
    {
        if (!isRight)     // <-
        {
            if (availableItemsSO.CurrentIndex == 0)                                 // At min index
                toLeft.SetActive(false);
            if (availableItemsSO.CurrentIndex == availableItemsSO.Count - 2) // At max index
                toRight.SetActive(true);
        }
        else if (isRight) // ->
        {
            if (availableItemsSO.CurrentIndex == availableItemsSO.Count - 1)     // At max index
                toRight.SetActive(false);
            if (availableItemsSO.CurrentIndex == 1)                           // At min index
                toLeft.SetActive(true);
        }
    }
    #endregion

}
