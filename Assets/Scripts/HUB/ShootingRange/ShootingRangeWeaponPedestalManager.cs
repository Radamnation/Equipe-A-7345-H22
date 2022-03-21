using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeWeaponPedestalManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Player Weapons Inventory")]
    [SerializeField] private WeaponsInventorySO playerWeaponInventorySO;

    [Header("Weapon Rack")]
                     private GameObject[] defaultWeapons;
    [SerializeField] private ArrayLinearGameObjectSO availableWeaponsSO;




    // SECTION - Method - Utility Specific ===================================================================
    private void SetArrayLinearGenerics()
    {
        //if (availableWeaponsSO.IsEmpty)
            //availableWeaponsSO.Copy(defaultWeapons);
    }

}
