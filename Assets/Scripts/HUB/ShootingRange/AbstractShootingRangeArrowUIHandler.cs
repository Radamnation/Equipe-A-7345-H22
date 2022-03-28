using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShootingRangeArrowUIHandler : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Manage If Tag")]
    [SerializeField] protected string triggerTag = "Player";

    [Header("To X UI Elements")]
    [SerializeField] protected GameObject toLeft;
    [SerializeField] protected GameObject toRight;

    protected List<GameObject> myInteractablePanels = new List<GameObject>();


    // SECTION - Method - Utility Specific ===================================================================
    public abstract void SetToUIVisual(bool isRight);

    protected void SetChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
            myInteractablePanels.Add(transform.GetChild(i).gameObject);
    }
}
