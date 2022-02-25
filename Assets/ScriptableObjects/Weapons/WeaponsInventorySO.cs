using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/WeaponInventory", fileName = "WeaponsInventorySO")]
public class WeaponsInventorySO : ScriptableObject
{
    [SerializeField] private int maxMainWeapons = 2;
    [SerializeField] private int maxSecodnaryWeapons = 1;

    [SerializeField] private List<WeaponSO> defaultMainWeapons = new List<WeaponSO>();
    [SerializeField] private List<WeaponSO> carriedMainWeapons = new List<WeaponSO>();
    [SerializeField] private WeaponSO equippedMainWeapon;

    [SerializeField] private List<WeaponSO> defaultSecondaryWeapons = new List<WeaponSO>();
    [SerializeField] private List<WeaponSO> carriedSecondaryWeapons = new List<WeaponSO>();
    [SerializeField] private WeaponSO equippedSecondaryWeapon;

    public int MaxMainWeapons { get => maxMainWeapons; set => maxMainWeapons = value; }
    public int MaxSecodnaryWeapons { get => maxSecodnaryWeapons; set => maxSecodnaryWeapons = value; }

    public List<WeaponSO> DefaultMainWeapons { get => defaultMainWeapons; set => defaultMainWeapons = value; }
    public List<WeaponSO> CarriedMainWeapons { get => carriedMainWeapons; set => carriedMainWeapons = value; }
    public WeaponSO EquippedMainWeapon { get => equippedMainWeapon; set => equippedMainWeapon = value; }

    public List<WeaponSO> DefaultSecondaryWeapons { get => defaultSecondaryWeapons; set => defaultSecondaryWeapons = value; }
    public List<WeaponSO> CarriedSecondaryWeapons { get => carriedSecondaryWeapons; set => carriedSecondaryWeapons = value; }
    public WeaponSO EquippedSecondaryWeapon { get => equippedSecondaryWeapon; set => equippedSecondaryWeapon = value; }

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
