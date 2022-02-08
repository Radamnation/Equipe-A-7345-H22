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

    public void ShootProjectile(GameObject projectile)
    {
        var newProjectile = Instantiate(projectile, transform);
        newProjectile.transform.parent = null;
        Debug.Log(newProjectile.name + " was instantiated");
    }

    public void ShootRayCast()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 1000);
        Debug.Log(hit.collider.name + " was hit");
        if (hit.collider.GetComponent<LivingEntityContext>() != null)
        {
            hit.collider.GetComponent<LivingEntityContext>().TakeDamage(1.0f);
        }
        else
        {
            var newBulletHole = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
        }
    }

    public void ShootMultipleRayCasts(int hits, float spread)
    {
        for (int i = 0; i < hits; i++)
        {
            RaycastHit hit;
            var spreadDirection = new Vector3(0, Random.Range(-spread, spread), Random.Range(-spread, spread));
            Physics.Raycast(transform.position, transform.forward + spreadDirection, out hit, 1000);
            Debug.Log(hit.collider.name + " was hit");
            if (hit.collider.GetComponent<LivingEntityContext>() != null)
            {
                hit.collider.GetComponent<LivingEntityContext>().TakeDamage(1.0f);
            }
            else
            {
                var newBulletHole = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal, Vector3.up));
            }
        }
    }
}
