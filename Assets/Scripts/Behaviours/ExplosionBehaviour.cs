using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExplosionBehaviour : MonoBehaviour
{
    // SECTION - Field ============================================================
    [Header("SphereCast Values")]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private bool isDebugOn = false;

    [Header("Behaviour Values")]
    [SerializeField] private bool delayDestroys = false;
    [SerializeField] private bool delayDisablesSprite = false;
    [SerializeField] private float executeDelay = 0.0f;
    [SerializeField] private float lingerDelay = 0.0f;
    [SerializeField] private float damage;

                     private Collider[] collArray;


    // SECTION - Method ============================================================
    private void FixedUpdate()
    {
        StaticRayCaster.RaycastSphereDebugger(transform.parent.transform, radius, isDebugOn);
    }

    public void Explosion()
    {
        Invoke("ExecuteExplosion", executeDelay);
    }

    private void ExecuteExplosion()
    {
        collArray = StaticRayCaster.IsOverlapSphereTouching(transform.position, radius, targetMask, isDebugOn); // transform.parent.transform

        foreach (Collider hitObj in collArray)
        {
            if (hitObj.GetComponent<LivingEntityContext>())
                hitObj.GetComponent<LivingEntityContext>().TakeDamage(damage);
            if (hitObj.GetComponent<BuildingBlock>())
                if (hitObj.GetComponent<BuildingBlock>().IsBreakable)
                    Destroy(hitObj.gameObject);
        }

        // Manage object / sprite renderer
        if (delayDisablesSprite)
            transform.parent.GetComponentInChildren<SpriteRenderer>().enabled = false;
        else if (delayDestroys)
            Invoke("AE_DestroyObject", lingerDelay);
    }

    private void AE_DestroyObject() // Animator Event
    {
        Destroy(transform.parent.gameObject);
    }
}
