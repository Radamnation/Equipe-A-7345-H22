using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactable : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private bool isInteractable = true;
    public UnityEvent interacted;


    // SECTION - Field ===================================================================
    public bool IsInteractable { get => isInteractable; }


    // SECTION - Method - Main ===================================================================
    public void OnInteraction()
    {
        interacted.Invoke();
    }


    // // SECTION - Method - Utility ===================================================================
    public void ToggleIsInteractable()
    {
        isInteractable = !isInteractable;
    }

    public void SetIsInteractable(bool setAs)
    {
        isInteractable = setAs;
    }

    public void ToggleInteractableLayer()
    {
        // 128 == 010000000 
        string interactableBinary = Convert.ToString(GameManager.instance.interactableMask.value, 2);
        string currBinary = Convert.ToString(LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer)), 2);

        // Prevent/Enable Interact popup window for this.gameObject
        if (currBinary == interactableBinary)
            gameObject.layer = LayerMask.GetMask("Default");
        else
            gameObject.layer = GameManager.instance.interactableMask;
    }
}
