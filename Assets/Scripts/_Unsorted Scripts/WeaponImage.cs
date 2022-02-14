using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponImage : MonoBehaviour
{
    [SerializeField] private WeaponsInventorySO weaponInventory;
    [SerializeField] private float shakeAmount = 30f;
    [SerializeField] private float shakeTime = 0.5f;

    private float shakeTimer = 0;
    private Vector3 initialPosition;
    private Image weaponImage;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        weaponImage = GetComponent<Image>();
        UpdateWeaponImage();
    }

    public void UpdateWeaponImage()
    {
        weaponImage.sprite = weaponInventory.EquippedMainWeapon.WeaponPlayerSprite;
    }

    public void ShakeWeapon()
    {
        shakeTimer = shakeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            var shakeOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            transform.position = initialPosition + shakeOffset;
        }
        else
        {
            transform.position = initialPosition;
        }
    }
}
