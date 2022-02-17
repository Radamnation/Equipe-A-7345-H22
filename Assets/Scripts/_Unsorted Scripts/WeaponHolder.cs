using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private BulletHole bulletHole;

    private float mainFireRateDelay;
    private float mainReloadDelay;
    private float secondaryFireRateDelay;

    public float MainFireRateDelay { get => mainFireRateDelay; set => mainFireRateDelay = value; }
    public float MainReloadDelay { get => mainReloadDelay; set => mainReloadDelay = value; }
    public float SecondaryFireRateDelay { get => secondaryFireRateDelay; set => secondaryFireRateDelay = value; }

    private void Update()
    {
        mainFireRateDelay -= Time.deltaTime;
        mainReloadDelay -= Time.deltaTime;
        secondaryFireRateDelay -= Time.deltaTime;
    }

    public void ShootProjectile(PhysicalProjectile projectile)
    {
        var newProjectile = Instantiate(projectile, transform);
        newProjectile.MyRigidbody.velocity += transform.parent.GetComponent<Rigidbody>().velocity * 0.25f;
        newProjectile.transform.parent = null;
        Debug.Log(newProjectile.name + " was instantiated");
    }

    public void ShootRayCast(float damage)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, GameManager.instance.canBeShotByPlayerMask, 1000);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name + " was hit");
            if (hit.collider.GetComponent<LivingEntityContext>() != null)
            {
                hit.collider.GetComponent<LivingEntityContext>().TakeDamage(damage);
            }
            else
            {
                var newBulletHole = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
            }
        }
        
    }

    public void ShootMultipleRayCasts(float damage, int hits, float spread)
    {
        for (int i = 0; i < hits; i++)
        {
            RaycastHit hit;
            var spreadDirection = new Vector3(0, Random.Range(-spread, spread), Random.Range(-spread, spread));
            Physics.Raycast(transform.position, transform.forward + spreadDirection, out hit, GameManager.instance.canBeShotByPlayerMask, 1000);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name + " was hit");
                if (hit.collider.GetComponent<LivingEntityContext>() != null)
                {
                    hit.collider.GetComponent<LivingEntityContext>().TakeDamage(damage / hits);
                }
                else
                {
                    var newBulletHole = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
                }
            }
        }
    }
}
