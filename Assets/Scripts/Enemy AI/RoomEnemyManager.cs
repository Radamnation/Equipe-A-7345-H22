using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

public class RoomEnemyManager : MonoBehaviour
{
    // SECTION - Field =========================================================
    [SerializeField] private bool isRoomShootingRange = false;

    [SerializeField] private TransformSO bossHealthBarCanvas;
    
    [SerializeField]
    private string[] doNotManageTheseTags =
    {
        "Practice Target"
    };

    private bool playerhasEntered = false;

    private BoxCollider myTriggerZone;

    private List<LivingEntityContext> myLivingEntityContextList = new List<LivingEntityContext>();

    private Room myRoom;



    // SECTION - Method - Unity Specific =========================================================
    private void Start()
    {
        myTriggerZone = GetComponent<BoxCollider>();
        if (!isRoomShootingRange)
        {
            myRoom = transform.parent.GetComponent<Room>();
            myTriggerZone.size = new Vector3(myRoom.XDimension-4, myRoom.XDimension-2, myRoom.ZDimension-4);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (isRoomShootingRange)
            Manager(other);

        if (!isRoomShootingRange && other.CompareTag("Player") && !myRoom.IsCompleted)
        {
            myRoom.CloseAllDoors();
            Debug.Log(other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (name == "ShootingRangeCenter" && other.CompareTag("Player"))
        {
            // TODO
            //      - Should destroy instantiated prefab of health bar inside a horizontal layout so that...
            //        multiple boss can easily destack (with a check for boss qty to not delete in the middle of a fight)
            bossHealthBarCanvas.Transform.GetChild(0).gameObject.SetActive(false);
            bossHealthBarCanvas.Transform.GetChild(1).gameObject.SetActive(false);

            other.GetComponent<LivingEntityContext>().FullHeal(); // Heal player in hub
            Reset();
        }

        if (!isRoomShootingRange)
            Manager(other);
    }


    // SECTION - Method - Utility Specific =========================================================
    public void Reset()
    {
        playerhasEntered = false;
        myLivingEntityContextList.Clear();
    }

    private void Manager(Collider other)
    {
        if (other.gameObject.layer == 8 && !playerhasEntered) // Layer int # for LIVING ENTITY
        {
            // Quit Check
            foreach (string tag in doNotManageTheseTags)
            {
                if (other.CompareTag(tag))
                    return;
            }

            LivingEntityContext otherLEC = other.GetComponent<LivingEntityContext>();

            if (otherLEC != null &&
                !otherLEC.ActivateEnemyOnTriggerEnter &&
                !myLivingEntityContextList.Contains(otherLEC))
            {
                myLivingEntityContextList.Add(otherLEC);
                ToggleActive(otherLEC);
            }
        }
        else if (other.CompareTag("Player") && !playerhasEntered)
        {
            playerhasEntered = true;

            foreach (LivingEntityContext otherLEC in myLivingEntityContextList)
            {
                ToggleActive(otherLEC, true);
            }
        }
    }

    public void ToggleActive(LivingEntityContext lec, bool forceActivation = false)
    {
        if (forceActivation)
        {
            lec.ActivateEnemyOnTriggerEnter = true;
            ToggleActive(lec);
        }
        else if (!CompareTag("Player"))
        {
            if (lec.ActivateEnemyOnTriggerEnter)
            {
                Enemy_SetComponents(lec, true, RigidbodyConstraints.FreezeRotation);
            }
            else
            {
                Enemy_SetComponents(lec, false, RigidbodyConstraints.FreezeAll);          
            }

            lec.ActivateEnemyOnTriggerEnter = !lec.ActivateEnemyOnTriggerEnter;
        }
    }

    public void Enemy_SetComponents(LivingEntityContext other, bool setAs, RigidbodyConstraints desiredRigidBodyConstraint)
    {
        Color desiredColor;
        if (setAs)
            desiredColor = Color.white;
        else
            desiredColor = Color.black;
        foreach (SpriteRenderer sprite in other.SpriteRenderer)
            sprite.color = desiredColor;

        other.GetComponent<Collider>().enabled = setAs;

        other.GetComponent<Rigidbody>().constraints = desiredRigidBodyConstraint;

        Animator anim = other.GetComponent<Animator>();
        if (anim)
            anim.enabled = setAs;


        if (other.CompareTag("Enemy"))
        {
            BasicEnemyContext enemyContext = other.GetComponent<BasicEnemyContext>();
            enemyContext.enabled = setAs;
            enemyContext.SetRigidBodyConstraint_Y();
            other.transform.GetChild(4).gameObject.SetActive(setAs); // Enemy Passives
            other.GetComponent<AIPath>().enabled = setAs;
        }
    }
}
