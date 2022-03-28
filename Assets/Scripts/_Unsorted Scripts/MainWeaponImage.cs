using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWeaponImage : MonoBehaviour
{
    [SerializeField] private WeaponsInventorySO weaponInventory;
    [SerializeField] private Vector3 reloadOffset;
    [SerializeField] private float shakeAmount = 30f;
    [SerializeField] private float shakeTime = 0.1f;

    private float shakeTimer = 0;
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private Image weaponImage;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        currentPosition = initialPosition;
        weaponImage = GetComponent<Image>();
        UpdateWeaponImage();
    }

    public void UpdateWeaponImage()
    {
        weaponImage.sprite = weaponInventory.EquippedMainWeapon.WeaponPlayerSprite;
    }

    public void ShakeWeapon()
    {
        weaponImage.sprite = weaponInventory.EquippedMainWeapon.WeaponFiringPlayerSprite;
        shakeTimer = shakeTime;
    }

    public void MoveGunDown()
    {
        weaponImage.color = Color.grey;
        currentPosition = initialPosition - reloadOffset;
        transform.position = currentPosition;
    }

    public void MoveGunUp()
    {
        weaponImage.color = Color.white;
        currentPosition = initialPosition;
        transform.position = currentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            var shakeOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            transform.position = currentPosition + shakeOffset * Time.timeScale;
        }
        else
        {
            weaponImage.sprite = weaponInventory.EquippedMainWeapon.WeaponPlayerSprite;
            transform.position = currentPosition;
        }
    }
}
