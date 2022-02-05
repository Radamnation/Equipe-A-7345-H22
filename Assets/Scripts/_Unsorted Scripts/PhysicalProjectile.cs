using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalProjectile : MonoBehaviour
{
    [SerializeField] private bool affectedByGravity = true;
    
    [SerializeField] private float impulsion = 5f;
    [SerializeField] private Vector3 impulsionDirection;

    [SerializeField] private float projectileSpeed;

    [SerializeField] private bool explodeOnImpact = false;

    [SerializeField] private bool asExplosionTimer;
    [SerializeField] private float explosionTimer;

    protected Rigidbody myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();

        myRigidbody.useGravity = affectedByGravity;

        if (impulsion > 0)
        {
            myRigidbody.AddForce((transform.forward + transform.up).normalized * impulsion, ForceMode.Impulse);
        }

        if (projectileSpeed > 0)
        {
            myRigidbody.velocity = transform.forward * projectileSpeed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (asExplosionTimer)
        {
            if (explosionTimer > 0)
            {
                explosionTimer -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!explodeOnImpact) return;

        Destroy(gameObject);
    }
}
