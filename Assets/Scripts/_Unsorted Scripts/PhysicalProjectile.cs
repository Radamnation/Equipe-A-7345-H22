using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalProjectile : MonoBehaviour
{
    // [SerializeField] private float colliderActivationDelay = 0.1f;
    [SerializeField] private bool isStartAsCollider = false;

    [SerializeField] private bool affectedByGravity = true;
    
    [SerializeField] private float impulsion = 5f;
    [SerializeField] private Vector3 impulsionDirection;

    [SerializeField] private float projectileSpeed;

    [SerializeField] private bool explodeOnImpact = false;

    [SerializeField] private bool asExplosionTimer;
    [SerializeField] private float explosionTimer;

    [SerializeField] private UnityEvent onDeathEvents;

    private Rigidbody myRigidbody;
    private Collider myCollider;

    public Rigidbody MyRigidbody { get => myRigidbody; set => myRigidbody = value; }
    public Collider MyCollider { get => myCollider; set => myCollider = value; }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();

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

    //private IEnumerator ActivateCollider()
    //{
    //    yield return new WaitForSeconds(colliderActivationDelay);
    //    myCollider.enabled = true;
    //}

    // Start is called before the first frame update
    void Start()
    {
        if (isStartAsCollider)
            myCollider.isTrigger = false;
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
                Death();               
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!explodeOnImpact) return;

        Death();
    }

    private void OnTriggerExit(Collider other)
    {
        myCollider.isTrigger = false;
    }

    public void Death()
    {
        onDeathEvents.Invoke();
        this.enabled = false;

        myRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        myCollider.enabled = false;
    }
}
