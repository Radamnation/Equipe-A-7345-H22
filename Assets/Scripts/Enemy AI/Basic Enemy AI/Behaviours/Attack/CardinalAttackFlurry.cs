using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardinalAttackFlurry : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Child Specific")]
    [SerializeField] private Transform myTransform;


    // SECTION - Method - Implementation Specific ===================================================================

    public override void Behaviour()
    {
        OnBehaviourStart();



    }

    public override bool ChildSpecificValidations()
    {
        return true;
    }


    // SECTION - Method - Utility Specific===================================================================
    private void OnBehaviourStart()
    {
        if (myTransform == null)
            myTransform = GetComponent<Transform>();
    }

    private IEnumerator ExecuteBehaviour()
    {

        WeaponManager myWeaponManager = myContext.GetCurrentWeaponManager();

        myWeaponManager.ShootProjectile(myWeaponManager.Weapon);

        yield return new WaitForSeconds(0.14f);

        RotateWeapon();
        myWeaponManager.ShootProjectile(myWeaponManager.Weapon);


        myContext.CanUseBehaviour = true;

        yield return null;
    }

    private void RotateWeapon()
    {
        float x = myTransform.localRotation.x + 90;
        float y = myTransform.localRotation.y;
        float z = myTransform.localRotation.z;
        float w = myTransform.localRotation.w;

        Quaternion myNewRotation = new Quaternion(x, y, z, w);
        myTransform.rotation = myNewRotation;
    }

    public void ShootProjectile(WeaponSO weapon)
    {
        var newProjectile = Instantiate(weapon.Projectile, transform);
        newProjectile.MyRigidbody.velocity += transform.parent.GetComponent<Rigidbody>().velocity * 0.25f;
        newProjectile.transform.parent = null;

        StaticDebugger.SimpleDebugger(true, newProjectile.name + " was instantiated");
    }
}
