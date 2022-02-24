using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/WeaponInventory", fileName = "WeaponsInventorySO")]
public class WeaponsInventorySO : ScriptableObject
{
    public List<WeaponSO> DefaultMainWeapons = new List<WeaponSO>();
    public List<WeaponSO> CarriedMainWeapons = new List<WeaponSO>();
    public WeaponSO EquippedMainWeapon;
    
    public List<WeaponSO> DefaultSecondaryWeapons = new List<WeaponSO>();
    public List<WeaponSO> CarriedSecondaryWeapons = new List<WeaponSO>();
    public WeaponSO EquippedSecondaryWeapon;

    // Start is called before the first frame update
    void OnEnable()
    {
        CarriedMainWeapons.Clear();
        for (int i = 0; i < DefaultMainWeapons.Count; i++)
        {
            CarriedMainWeapons.Add(Instantiate(DefaultMainWeapons[i]));
        }
        EquippedMainWeapon = CarriedMainWeapons[0];
        CarriedSecondaryWeapons.Clear();
        for (int i = 0; i < DefaultSecondaryWeapons.Count; i++)
        {
            CarriedSecondaryWeapons.Add(Instantiate(DefaultSecondaryWeapons[i]));
        }
        EquippedSecondaryWeapon = CarriedSecondaryWeapons[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
