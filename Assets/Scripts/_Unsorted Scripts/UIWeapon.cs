using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWeapon : MonoBehaviour
{
    [SerializeField] private WeaponsInventorySO weaponsInventory;
    [SerializeField] private bool isSecondary = false;

    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI ammoValue;
    [SerializeField] private TextMeshProUGUI clipValue;

    public void UpdateValue()
    {
        if (!isSecondary)
        {
            weaponImage.sprite = weaponsInventory.EquippedMainWeapon.WeaponUISprite;
            ammoValue.text = weaponsInventory.EquippedMainWeapon.CurrentAmmo + " / " + weaponsInventory.EquippedMainWeapon.MaxAmmo;
            clipValue.text = weaponsInventory.EquippedMainWeapon.CurrentClip + " / " + weaponsInventory.EquippedMainWeapon.MaxClip;
        }
        else
        {
            weaponImage.sprite = weaponsInventory.EquippedSecondaryWeapon.WeaponUISprite;
            clipValue.text = weaponsInventory.EquippedSecondaryWeapon.CurrentClip + " / " + weaponsInventory.EquippedSecondaryWeapon.MaxClip;
        }
    }
}
