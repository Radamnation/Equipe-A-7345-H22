using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    [SerializeField] private PickableSO pickableSO;

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
        if (collision.gameObject.GetComponentInChildren<PickableManager>() != null)
        {
            ActivatePickable(collision);
        }
    }

    private void ActivatePickable(Collision collision)
    {
        if (collision.gameObject.GetComponentInChildren<PickableManager>().PickPickable(pickableSO))
        {
            Destroy(gameObject);
        }
    }
}
