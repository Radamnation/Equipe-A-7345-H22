using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    [SerializeField] private PickableSO pickableSO;
    [SerializeField] private WeaponsInventorySO weaponsInventorySO;

    [SerializeField] private FloatReference health;
    [SerializeField] private FloatReference armor;
    [SerializeField] private FloatReference currency;

    [SerializeField] private UnityEvent healthAsChange;
    [SerializeField] private UnityEvent armorAsChange;
    [SerializeField] private UnityEvent ammoAsChange;
    [SerializeField] private UnityEvent secondaryAsChange;
    [SerializeField] private UnityEvent currencyAsChange;

    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        mySpriteRenderer.sprite = pickableSO.Sprite;
        mySpriteRenderer.color = pickableSO.Color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerContext>() != null)
        {
            ActivatePickable();
            Destroy(gameObject);
        }
    }

    private void ActivatePickable()
    {
        if (pickableSO.HealthValue > 0)
        {
            if (pickableSO.HealthValueIsPercent)
            {

            }
            else
            {
                health.Value += pickableSO.HealthValue;
            }
            healthAsChange.Invoke();
        }
        if (pickableSO.ArmorValue > 0)
        {
            if (pickableSO.ArmorValueIsPercent)
            {

            }
            else
            {
                armor.Value += pickableSO.ArmorValue;
            }
            armorAsChange.Invoke();
        }
        if (pickableSO.AmmoValue > 0)
        {
            if (pickableSO.AmmoValueIsPercent)
            {

            }
            else
            {
                weaponsInventorySO.EquippedMainWeapon.CurrentAmmo += (int)pickableSO.AmmoValue;
            }
            ammoAsChange.Invoke();
        }
        if (pickableSO.SecondaryValue > 0)
        {
            if (pickableSO.SecondaryValueIsPercent)
            {

            }
            else
            {
                weaponsInventorySO.EquippedSecondaryWeapon.CurrentAmmo += (int)pickableSO.SecondaryValue;
            }
            secondaryAsChange.Invoke();
        }
        if (pickableSO.CurrencyValue > 0)
        {
            currency.Value += pickableSO.CurrencyValue;
            currencyAsChange.Invoke();
        }
    }
}
