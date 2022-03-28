using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// NOTE
//      - ADD & CHANGE methods could be refactored into single generic-ish method (enum?) to aleviate eye bleeding

[CreateAssetMenu(menuName = "Scriptable/Weapons/WeaponInventory", fileName = "WeaponsInventorySO")]
public class WeaponsInventorySO : ScriptableObject
{
    [SerializeField] private GameEvent mainWeaponHasChangedGE;
    [SerializeField] private GameEvent secondaryWeaponHasChangedGE;

    [SerializeField] private int maxMeleeWeapons = 1;
    [SerializeField] private int maxMainWeapons = 2;
    [SerializeField] private int maxSecodnaryWeapons = 1;

    [SerializeField] private List<WeaponSO> defaultMeleeWeapons = new List<WeaponSO>();
    [SerializeField] private List<WeaponSO> carriedMeleeWeapons = new List<WeaponSO>();
    [SerializeField] private WeaponSO equippedMeleeWeapon;

    [SerializeField] private List<WeaponSO> defaultMainWeapons = new List<WeaponSO>();
    [SerializeField] private List<WeaponSO> carriedMainWeapons = new List<WeaponSO>();
    [SerializeField] private WeaponSO equippedMainWeapon;

    [SerializeField] private List<WeaponSO> defaultSecondaryWeapons = new List<WeaponSO>();
    [SerializeField] private List<WeaponSO> carriedSecondaryWeapons = new List<WeaponSO>();
    [SerializeField] private WeaponSO equippedSecondaryWeapon;

    public int MaxMeleeWeapons { get => maxMeleeWeapons; set => maxMeleeWeapons = value; }
    public int MaxMainWeapons { get => maxMainWeapons; set => maxMainWeapons = value; }
    public int MaxSecodnaryWeapons { get => maxSecodnaryWeapons; set => maxSecodnaryWeapons = value; }

    public List<WeaponSO> DefaultMeleeWeapons { get => defaultMeleeWeapons; set => defaultMeleeWeapons = value; }
    public List<WeaponSO> CarriedMeleeWeapons { get => carriedMeleeWeapons; set => carriedMeleeWeapons = value; }
    public WeaponSO EquippedMeleeWeapon { get => equippedMeleeWeapon; set => equippedMeleeWeapon = value; }

    public List<WeaponSO> DefaultMainWeapons { get => defaultMainWeapons; set => defaultMainWeapons = value; }
    public List<WeaponSO> CarriedMainWeapons { get => carriedMainWeapons; set => carriedMainWeapons = value; }
    public WeaponSO EquippedMainWeapon { get => equippedMainWeapon; set => equippedMainWeapon = value; }

    public List<WeaponSO> DefaultSecondaryWeapons { get => defaultSecondaryWeapons; set => defaultSecondaryWeapons = value; }
    public List<WeaponSO> CarriedSecondaryWeapons { get => carriedSecondaryWeapons; set => carriedSecondaryWeapons = value; }
    public WeaponSO EquippedSecondaryWeapon { get => equippedSecondaryWeapon; set => equippedSecondaryWeapon = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        SetDefaultWeapons();
    }

    public void SetDefaultWeapons()
    {
        CarriedMeleeWeapons.Clear();
        for (int i = 0; i < DefaultMeleeWeapons.Count; i++)
        {
            CarriedMeleeWeapons.Add(Instantiate(DefaultMeleeWeapons[i]));
        }
        EquippedMeleeWeapon = CarriedMeleeWeapons[0];

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

    public void AddWeapon_Main(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        if (carriedMainWeapons.Count >= maxMainWeapons)
            return;

        carriedMainWeapons.Add(Instantiate(otherWeaponSO));

        if (alsoEquip)
        {
            equippedMainWeapon = carriedMainWeapons[carriedMainWeapons.Count-1];

            if (mainWeaponHasChangedGE != null)
                mainWeaponHasChangedGE.Raise();
        }
    }

    public void ChangeWeapon_Main(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        // Find Weapon from carried list and switcharoo with pedestal weapon before equipping it
        for (int i = 0; i < carriedMainWeapons.Count; i++)
            if (carriedMainWeapons[i].GetInstanceID() == equippedMainWeapon.GetInstanceID())
            {
                Destroy(carriedMainWeapons[i]);
                carriedMainWeapons[i] = Instantiate(otherWeaponSO);

                if (alsoEquip)
                {
                    equippedMainWeapon = carriedMainWeapons[i];

                    if (mainWeaponHasChangedGE != null)
                        mainWeaponHasChangedGE.Raise();
                }

                break;
            }
    }


    public void AddWeapon_Secondary(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        if (carriedSecondaryWeapons.Count >= maxSecodnaryWeapons)
            return;

        carriedSecondaryWeapons.Add(Instantiate(otherWeaponSO));

        if (alsoEquip)
        {
            equippedSecondaryWeapon = carriedSecondaryWeapons[carriedSecondaryWeapons.Count - 1];

            if (secondaryWeaponHasChangedGE != null)
                secondaryWeaponHasChangedGE.Raise();
        }

    }

    public void ChangeWeapon_Secondary(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        // Find Weapon from carried list and switcharoo with pedestal weapon before equipping it
        for (int i = 0; i < carriedSecondaryWeapons.Count; i++)
            if (carriedSecondaryWeapons[i].GetInstanceID() == equippedSecondaryWeapon.GetInstanceID())
            {
                Destroy(carriedSecondaryWeapons[i]);
                carriedSecondaryWeapons[i] = Instantiate(otherWeaponSO);

                if (alsoEquip)
                {
                    equippedSecondaryWeapon = carriedSecondaryWeapons[i];

                    if (secondaryWeaponHasChangedGE != null)
                        secondaryWeaponHasChangedGE.Raise();

                    Debug.Log("RAISED");
                    Debug.Log($"Clip: {equippedSecondaryWeapon.CurrentClip}");
                }
                    
                break;
            }
    }


    public void AddWeapon_Melee(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        if (carriedMeleeWeapons.Count >= maxMeleeWeapons)
            return;

        carriedMeleeWeapons.Add(Instantiate(otherWeaponSO));

        if (alsoEquip)
            equippedMeleeWeapon = carriedMeleeWeapons[carriedMeleeWeapons.Count-1];
    }

    public void ChangeWeapon_Melee(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        // Find Weapon from carried list and switcharoo with pedestal weapon before equipping it
        for (int i = 0; i < carriedMeleeWeapons.Count; i++)
            if (carriedMeleeWeapons[i].GetInstanceID() == equippedMeleeWeapon.GetInstanceID())
            {
                Destroy(carriedMeleeWeapons[i]);
                carriedMeleeWeapons[i] = Instantiate(otherWeaponSO);

                if (alsoEquip)
                    equippedMeleeWeapon = carriedMeleeWeapons[i];

                break;
            }
    }
}
