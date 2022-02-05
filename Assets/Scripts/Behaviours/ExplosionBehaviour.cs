using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    // SECTION - Field ============================================================
    [Header("SphereCast Values")]
    [SerializeField] private float radius;
    //[SerializeField] private float maxDistance;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private bool isDebugOn = false;

    [Header("Behaviour Values")]
    [SerializeField] private float delay = 0.0f;
    [SerializeField] private float damage;

    private Collider[] collArray;


    // SECTION - Method ============================================================
    private void FixedUpdate()
    {
        StaticRayCaster.RaycastSphereDebugger(transform.parent.transform, radius, isDebugOn);
    }

    public void Explosion()
    {
        Invoke("ExecuteExplosion", delay);
    }

    public void ExecuteExplosion()
    {
        collArray = StaticRayCaster.IsOverlapSphereTouching(transform.parent.transform, radius, targetMask, isDebugOn);

        foreach (Collider hitObj in collArray)
            if (hitObj.GetComponent<LivingEntityContext>())
                hitObj.GetComponent<LivingEntityContext>().TakeDamage(damage);
    }
}
