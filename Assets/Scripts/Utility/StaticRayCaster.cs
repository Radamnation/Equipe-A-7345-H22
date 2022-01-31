using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRayCaster : MonoBehaviour
{
    // SECTION - Method =========================================================
    #region REGION - LINE RAYCAST
    static public RaycastHit IsTouching(Vector3 origin, Vector3 direction, float distance, LayerMask mask, bool isDebugOn)
    {
        if (isDebugOn)
        {
            float endX = origin.x + direction.x * distance;
            float endY = origin.y + direction.y * distance;
            float endZ = origin.z + direction.z * distance;
            Vector3 end = new(endX, endY, endZ);

            Debug.DrawLine(origin, end, Color.yellow);
        }

        Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask);
        return hit;
    }

    static public RaycastHit[] IsTouchingMultiple(Vector3 origin, Vector3 direction, float distance, LayerMask mask, bool isDebugOn)
    {
        if (isDebugOn)
        {
            float endX = origin.x + direction.x * distance;
            float endY = origin.y + direction.y * distance;
            float endZ = origin.z + direction.z * distance;
            Vector3 end = new(endX, endY, endZ);

            Debug.DrawLine(origin, end, Color.yellow);
        }

        return Physics.RaycastAll(origin, direction, distance, mask);
    }
    #endregion

}
