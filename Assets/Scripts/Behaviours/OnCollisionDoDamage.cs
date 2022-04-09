using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionDoDamage : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Damage Output")]
    [SerializeField] private WeaponSO myWeapon;
    [SerializeField] private float additionalDamage = 0;

    private float GetTotalDamageOutput => (myWeapon) ? myWeapon.Damage + additionalDamage : additionalDamage;


    // SECTION - Method - Unity Specific ===================================================================
    private void OnCollisionEnter(Collision other)
    {
        LivingEntityContext otherLEC = other.transform.GetComponent<LivingEntityContext>();

        if (otherLEC != null)
            otherLEC.TakeDamage(GetTotalDamageOutput);
    }
}
