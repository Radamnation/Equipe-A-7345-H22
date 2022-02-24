using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pedestal : MonoBehaviour
{
    [SerializeField] private float spriteRotationSpeed = 0.5f;
    [SerializeField] private float spriteVerticalRange = 0.1f;
    [SerializeField] private float spriteVerticalSpeed = 1f;

    [SerializeField] private WeaponSO[] randomWeapons;
    [SerializeField] private WeaponsInventorySO weaponInventory;
    [SerializeField] private UnityEvent mainWeaponHasChanged;

    private WeaponSO pedestalWeapon;
    private SpriteRenderer mySpriteRenderer;
    private Vector3 spriteInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteInitialPosition = mySpriteRenderer.transform.localPosition;
        pedestalWeapon = Instantiate(randomWeapons[Random.Range(0, randomWeapons.Length)]);
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        mySpriteRenderer.transform.Rotate(new Vector3(0, spriteRotationSpeed, 0));
        var verticalOffset = Mathf.Sin(Time.time * spriteVerticalSpeed) * spriteVerticalRange;
        mySpriteRenderer.transform.localPosition = spriteInitialPosition + new Vector3(0, verticalOffset, 0);
    }

    public void ActivatePedestal()
    {
        WeaponSO temp = pedestalWeapon;
        pedestalWeapon = weaponInventory.EquippedMainWeapon;
        var carriedWeaponIndex = weaponInventory.CarriedMainWeapons.IndexOf(weaponInventory.EquippedMainWeapon);
        weaponInventory.CarriedMainWeapons[carriedWeaponIndex] = temp;
        weaponInventory.EquippedMainWeapon = temp;
        mainWeaponHasChanged.Invoke();
        UpdateSprite();
        // mySpriteRenderer.enabled = false;
    }

    private void UpdateSprite()
    {
        mySpriteRenderer.sprite = pedestalWeapon.WeaponUISprite;
    }
}
