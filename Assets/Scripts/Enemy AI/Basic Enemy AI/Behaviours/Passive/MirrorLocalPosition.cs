using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorLocalPosition : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Child Specific")]
    [SerializeField] private Transform myTransform;

    [Header("Modifiables")]
    [SerializeField] private bool mirrorX = false;
    [SerializeField] private bool mirrorY = false;
    [SerializeField] private bool mirrorZ = false;


    // SECTION - Method - Implementation Specific ===================================================================

    public override void Behaviour()
    {
        OnBehaviourStart();

        float x = myTransform.localPosition.x;
        float y = myTransform.localPosition.y;
        float z = myTransform.localPosition.z;

        if(mirrorX)
            x *= -1;

        if (mirrorY)
            y *= -1;

        if (mirrorZ)
            z *= -1;

        Vector3 newPos = new Vector3(x, y, z);

        myTransform.localPosition = newPos;

        myContext.CanUseBehaviour = true;
    }

    public override bool ChildSpecificValidations()
    {
        return true;
    }

    // SECTION - Method - Utility Specific===================================================================
    private void OnBehaviourStart()
    {
        if (myTransform == null)
            myTransform = GetComponent<Transform>();
    }
}
