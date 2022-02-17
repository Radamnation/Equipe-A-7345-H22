using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableCanvas : MonoBehaviour
{
    [SerializeField] private float flashTime = 0.1f;
    [SerializeField] private float colorAlpha = 0.25f;

    private Image myImage;

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        myImage.enabled = false;
    }

    public void FlashHealthColor()
    {
        StartCoroutine(StartFlash(Color.red));
    }

    public void FlashArmorColor()
    {
        StartCoroutine(StartFlash(Color.blue));
    }

    public void FlashAmmoColor()
    {
        StartCoroutine(StartFlash(Color.yellow));
    }

    public void FlashSecondaryColor()
    {
        StartCoroutine(StartFlash(Color.white));
    }

    public void FlashCurrencyColor()
    {
        StartCoroutine(StartFlash(Color.green));
    }

    private IEnumerator StartFlash(Color color)
    {
        color.a = colorAlpha;
        myImage.color = color;
        myImage.enabled = true;
        yield return new WaitForSeconds(flashTime);
        myImage.enabled = false;
    }
}
