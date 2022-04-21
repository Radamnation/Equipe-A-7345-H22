using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPedestal : MonoBehaviour
{
    [SerializeField] private float spriteRotationSpeed = 0.5f;
    [SerializeField] private float spriteVerticalRange = 0.1f;
    [SerializeField] private float spriteVerticalSpeed = 1f;

    [SerializeField] private WeaponSO specificWeapon;
    [SerializeField] private bool isInHub;
    [SerializeField] private bool weaponIsRandom = true;
    [SerializeField] private WeaponSO[] randomWeapons;
    [SerializeField] private WeaponsInventorySO weaponInventory;
    [SerializeField] private UnityEvent mainWeaponHasChanged;

    [SerializeField] private ArrayLinearWeaponSOSO hubWeaponArray_Main;
    [SerializeField] private ArrayLinearWeaponSOSO hubWeaponArray_Secondary;
    [SerializeField] private ArrayLinearWeaponSOSO hubWeaponArray_Melee;

    private WeaponSO pedestalWeapon;
    private SpriteRenderer mySpriteRenderer;
    private Vector3 spriteInitialPosition;
    private System.Random roomGenerationRandom;

    int randomIndex;
    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteInitialPosition = mySpriteRenderer.transform.localPosition;
        roomGenerationRandom = RandomManager.instance.RoomGenerationRandom.SystemRandom;
        if (weaponIsRandom)
        {
            randomIndex = roomGenerationRandom.Next(0, randomWeapons.Length);
            pedestalWeapon = Instantiate(randomWeapons[randomIndex]);
        }
        else
        {
            pedestalWeapon = Instantiate(specificWeapon);
        }
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
        var otherPedestals = transform.parent.transform.GetComponentsInChildren<WeaponPedestal>();
        if (!isInHub)
        {
            foreach (WeaponPedestal pedestal in otherPedestals)
            {
                if (pedestal != this)
                {
                    pedestal.EmptyPedestal();
                }
            }
        }    
        
        if (weaponInventory.CarriedMainWeapons.Count < weaponInventory.MaxMainWeapons && !isInHub)
        {
            WeaponSO temp = pedestalWeapon;

            /// <NOTE>
            /// 
            /// If conditional is a TEMPORARY DEBUGGER
            ///     - Get rid of conditional when merging of weaponPedestal.cs with ShootingRangePedestal.cs
            /// 
            /// </NOTE>
            if (!pedestalWeapon.name.Contains("Melee") && !pedestalWeapon.name.Contains("Grenade")) // !pedestalWeapon.HasPlayerUsedOnce && 
            {
                hubWeaponArray_Main.AddUnique(randomWeapons[randomIndex]); // Hub linear array update
            }

            pedestalWeapon = null;
            weaponInventory.CarriedMainWeapons.Add(temp);
            weaponInventory.EquippedMainWeapon = temp;
            mainWeaponHasChanged.Invoke();
            EmptyPedestal();
        }
        else
        {
            WeaponSO temp = pedestalWeapon;
            pedestalWeapon = weaponInventory.EquippedMainWeapon;
            var carriedWeaponIndex = weaponInventory.CarriedMainWeapons.IndexOf(weaponInventory.EquippedMainWeapon);
            weaponInventory.CarriedMainWeapons[carriedWeaponIndex] = temp;
            weaponInventory.EquippedMainWeapon = temp;
            mainWeaponHasChanged.Invoke();
            UpdateSprite();
        }
        // mySpriteRenderer.enabled = false;
    }

    private void UpdateSprite()
    {
        mySpriteRenderer.sprite = pedestalWeapon.WeaponUISprite;
    }

    public void EmptyPedestal()
    {
        mySpriteRenderer.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
    }
}
