using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/WeaponInventory", fileName = "WeaponsInventorySO")]
public class WeaponsInventorySO : ScriptableObject
{
    public List<WeaponSO> CarriedMainWeapons = new List<WeaponSO>();
    public WeaponSO EquippedMainWeapon;
    
    public List<WeaponSO> CarriedSecondaryWeapons = new List<WeaponSO>();
    public WeaponSO EquippedSecondaryWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
