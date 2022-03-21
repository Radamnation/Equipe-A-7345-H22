using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeWeaponPedestalManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Player Weapons Inventory")]
    [SerializeField] private WeaponsInventorySO playerWeaponInventorySO;

    [Header("Weapon Rack")]
                     private List<WeaponSO> defaultWeapons;
    [SerializeField] private WeaponsInventorySO playerWeaponInventory;
    [SerializeField] private ArrayLinearWeaponSOSO availableWeaponsSO;




    // SECTION - Method - Utility Specific ===================================================================
    private void Awake()
    {
        // Set default weapon in case of reset to weapons uppon entering HUB
        /*
        if (playerWeaponInventory.CarriedMainWeapons != null)
            foreach (WeaponSO weapon in playerWeaponInventory.CarriedMainWeapons)
            {
                defaultWeapons.Add(weapon);
            }

        if (playerWeaponInventory.CarriedSecondaryWeapons != null)
            foreach (WeaponSO weapon in playerWeaponInventory.CarriedSecondaryWeapons)
            {
                defaultWeapons.Add(weapon);
            }
        */
    }


    private void SetArrayLinearGenerics()
    {
        //if (availableWeaponsSO.IsEmpty)
            //availableWeaponsSO.Copy(defaultWeapons);
    }

}
