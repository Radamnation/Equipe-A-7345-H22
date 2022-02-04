using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIVariableBar : MonoBehaviour
{
    [SerializeField] private FloatReference maxValue;
    [SerializeField] private FloatReference currentValue;

    [SerializeField] private Image imageValue;
    [SerializeField] private TextMeshProUGUI textValue;

    public void UpdateValue()
    {
        imageValue.fillAmount = currentValue.Variable.Value / maxValue.Variable.Value;
        textValue.text = currentValue.Variable.Value + " / " + maxValue.Variable.Value;
    }
}
