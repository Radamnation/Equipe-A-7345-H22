using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePanel : MonoBehaviour
{
    [SerializeField] private float flashTime = 0.1f;
    [SerializeField] private Image northImage;
    [SerializeField] private Image southImage;
    [SerializeField] private Image eastImage;
    [SerializeField] private Image westImage;

    // Start is called before the first frame update
    void Start()
    {
        northImage.enabled = false;
        southImage.enabled = false;
        eastImage.enabled = false;
        westImage.enabled = false;
    }

    public void ReceiveDamage()
    {
        StartCoroutine(StartFlash());
    }

    private IEnumerator StartFlash()
    {
        northImage.enabled = true;
        southImage.enabled = true;
        eastImage.enabled = true;
        westImage.enabled = true;
        yield return new WaitForSeconds(flashTime);
        northImage.enabled = false;
        southImage.enabled = false;
        eastImage.enabled = false;
        westImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
