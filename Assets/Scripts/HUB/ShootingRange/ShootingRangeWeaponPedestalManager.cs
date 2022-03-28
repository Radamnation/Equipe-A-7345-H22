using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShootingRangeWeaponPedestalManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Weapons Inventory Scriptable")]
    [SerializeField] private WeaponsInventorySO playerWeaponInventorySO;

    [Header("Weapon Rack")]
    [SerializeField] private Sprite[] AddChangeCue;
    [Space(10)]
    [SerializeField] private WeaponSO pedestalWeapon;
    [SerializeField] private ArrayLinearWeaponSOSO availableWeaponsSO;
                     private bool isAddToInventory = false;
                     private Image mySpriteRenderer;
                     private SpriteRenderer AddOrChangeSprite;


    // SECTION - Property ===================================================================
    public ArrayLinearWeaponSOSO AvailableWeaponsSO { get => availableWeaponsSO; }


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        mySpriteRenderer = transform.GetChild(1).GetComponentInChildren<Image>();
        AddOrChangeSprite = transform.GetChild(3).GetComponent<SpriteRenderer>();
        pedestalWeapon = availableWeaponsSO.GetElement(0);
        UpdatePedestalSprite();
    }


    // SECTION - Method - Utility Specific ===================================================================
    public void ToggleAddOrChange()
    {
        isAddToInventory = !isAddToInventory;
        AddOrChangeSprite.sprite = AddChangeCue[(isAddToInventory) ? 1 : 0];
    }

    public void ChangePedestalWeapon_LeftOrRight(bool getNext = false)
    {
        if (getNext)
            pedestalWeapon = availableWeaponsSO.GetNext();
        else if (!getNext)
            pedestalWeapon = availableWeaponsSO.GetPrevious();

        UpdatePedestalSprite();
    }

    private void UpdatePedestalSprite()
    {
        mySpriteRenderer.sprite = pedestalWeapon.WeaponUISprite;
    }

    // Main weapon
    #region REGION - Main Weapon
    public void AddOrChange_MainWeapon()
    {
        if (isAddToInventory)
            AddMainWeapon();
        else
            ChangeMainWeapon();
    }

    private void AddMainWeapon()
    {
        playerWeaponInventorySO.AddWeapon_Main(pedestalWeapon);
    }

    private void ChangeMainWeapon()
    {
        playerWeaponInventorySO.ChangeWeapon_Main(pedestalWeapon);
    }
    #endregion

    // Secondary Weapon
    #region REGION - Secondary Weapon
    public void AddOrChange_SecondaryWeapon()
    {
        if (isAddToInventory)
            AddSecondaryWeapon();
        else
            ChangeSecondaryWeapon();
    }

    private void AddSecondaryWeapon()
    {
        playerWeaponInventorySO.AddWeapon_Secondary(pedestalWeapon);
    }

    private void ChangeSecondaryWeapon()
    {
        playerWeaponInventorySO.ChangeWeapon_Secondary(pedestalWeapon);
    }
    #endregion

    // Melee Weapon
    #region REGION - Melee Weapon
    public void AddOrChange_MeleeWeapon()
    {
        if (isAddToInventory)
            AddMeleeWeapon();
        else
            ChangeMeleeWeapon();
    }

    private void AddMeleeWeapon()
    {
        playerWeaponInventorySO.AddWeapon_Melee(pedestalWeapon);
    }

    private void ChangeMeleeWeapon()
    {
        playerWeaponInventorySO.ChangeWeapon_Melee(pedestalWeapon);
    }
    #endregion
}
