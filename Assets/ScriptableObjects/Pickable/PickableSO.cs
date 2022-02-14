using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Pickables/Pickable", fileName = "PickableSO")]
public class PickableSO : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Color color;
    [SerializeField] private bool healthValueIsPercent;
    [SerializeField] private float healthValue;
    [SerializeField] private bool armorValueIsPercent;
    [SerializeField] private float armorValue;
    [SerializeField] private bool ammoValueIsPercent;
    [SerializeField] private float ammoValue;
    [SerializeField] private bool secondaryValueIsPercent;
    [SerializeField] private float secondaryValue;
    [SerializeField] private float currencyValue;

    public Sprite Sprite { get => sprite; }
    public Color Color { get => color; }
    public bool HealthValueIsPercent { get => healthValueIsPercent; }
    public float HealthValue { get => healthValue; }
    public bool ArmorValueIsPercent { get => armorValueIsPercent; }
    public float ArmorValue { get => armorValue; }
    public bool AmmoValueIsPercent { get => ammoValueIsPercent; }
    public float AmmoValue { get => ammoValue; }
    public bool SecondaryValueIsPercent { get => secondaryValueIsPercent; }
    public float SecondaryValue { get => secondaryValue; }
    public float CurrencyValue { get => currencyValue; }
}
