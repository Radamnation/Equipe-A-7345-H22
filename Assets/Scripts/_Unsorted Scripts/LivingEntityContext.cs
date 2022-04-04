using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using UnityEngine.UI;

public class LivingEntityContext : MonoBehaviour
{
    // SECTION - Field =========================================================
    [Header("Health")]
    [SerializeField] private FloatReference maxHP;
    [SerializeField] private FloatReference currentHP;
    [SerializeField] private FloatReference maxArmor;
    [SerializeField] private FloatReference currentArmor;

    [Header("Animator")]
    [Tooltip("You may need to add [AB_ManageOnDeathAnim.cs] to animation state")]
    [SerializeField] private bool exitDeathDestroys = false;
    [Tooltip("You may need to add [AB_ManageOnDeathAnim.cs] to animation state")]
    [SerializeField] private bool exitDeathDisablesSprite = false;
    [Space(10)]
    [SerializeField] private float onHitCueDuration = 0.14f;
    [SerializeField] private Animator anim;
    [SerializeField] private string deathAnimStr;
    [SerializeField] private string takeDmgAnimStr;

    [Header("Events")]
    [SerializeField] private UnityEvent onTakeDamageEvents;
    [SerializeField] private UnityEvent onDeathEvents;

                     private SpriteRenderer[] spriteRenderer;
                     private Rigidbody myRigidbody;

    [Header("Enemy Section")] // isEnemy should be an editor variable which hides all Enemy variables if false
    [SerializeField] private bool isEnemy = true;
    [Tooltip("Enemy Specific bool:\nEnemy deactivate upon entering a trigger zone managed by [RooomEnemyManager.cs]")]
    [SerializeField] private bool activateEnemyOnTriggerEnter = false;


    [Header("Drops")]
    [SerializeField] private Pickable[] myDrops;
    [SerializeField] private int minDrop = 5;
    [SerializeField] private int maxDrop = 10;

    [Header("SFX")]
    [SerializeField] private AudioClip[] myHurtSFX;
    [SerializeField] private AudioClip myDeathSFX;

    private AudioSource myAudioSource;

    // SECTION - Property =========================================================
    public bool IsDead { get => currentHP.Value <= 0.0f; }
    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }
    public bool ActivateEnemyOnTriggerEnter { get => activateEnemyOnTriggerEnter; set => activateEnemyOnTriggerEnter = value; }
    public SpriteRenderer[] SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }


    // SECTION - Method - Unity Specific =========================================================
    private void Start()
    {
        FullHeal();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }


    // SECTION - Method - Context Specific =========================================================
    public void FullHeal()
    {
        currentHP.Value = maxHP.Value;
        if (gameObject.CompareTag("Player") && onTakeDamageEvents != null)
        {
            onTakeDamageEvents.Invoke();
        }
    }

    public void FullArmor()
    {
        currentArmor.Value = maxArmor.Value;
        if (onTakeDamageEvents != null)
        {
            onTakeDamageEvents.Invoke();
        }
    }

    public void KnockBack(float knockback, Vector3 direction)
    {
        myRigidbody.AddForce(knockback * direction, ForceMode.Impulse);
    }

    public void TakeDamage(float damage)
    {
        if (currentArmor.Value > 0.0f)
        {
            currentArmor.Value -= damage;
            if (currentArmor.Value < 0)
            {
                damage = -currentArmor.Value;
                currentArmor.Value = 0;
            }
            else
            {
                damage = 0;
            }
        }
        if (currentHP.Value > 0.0f)
        {
            currentHP.Value -= damage;

            StartCoroutine(TakeDamageVisualCue());

            // On Death
            if (IsDead)
            {
                if (myAudioSource != null)
                {
                    myAudioSource.PlayOneShot(myDeathSFX);
                }
                currentHP.Value = 0;
                OnDeathBaseHandler(); // Placed here to avoid manual storing in event
                if (onTakeDamageEvents != null)
                {
                    onTakeDamageEvents.Invoke();
                }
                if (onDeathEvents != null)
                {
                    onDeathEvents.Invoke();
                }
            }
            // On Simple Damage
            else
            {
                if (myAudioSource != null)
                {
                    myAudioSource.PlayOneShot(myHurtSFX[Random.Range(0, myHurtSFX.Length)]);
                }
                OnTakeDamageBaseHandler(); // Placed here to avoid manual storing in event
                if (onTakeDamageEvents != null)
                    onTakeDamageEvents.Invoke();
            }
        }
    }

    public void InstantDeath()
    {
        TakeDamage(maxHP.Value);
    }


    // SECTION - Method - Utility Specific =========================================================
    private void OnDeathBaseHandler() 
    {
        // Animator
        if (anim != null && deathAnimStr != "")
            anim.SetTrigger(deathAnimStr);
    }

    private void OnTakeDamageBaseHandler()
    {
        // Animator
        if (anim != null && takeDmgAnimStr != "")
            anim.SetTrigger(takeDmgAnimStr);

        // Extend default behaviours on take damage here
    }

    private IEnumerator TakeDamageVisualCue()
    {
        // Red
        foreach (SpriteRenderer renderer in spriteRenderer)
            renderer.color = Color.red;

        yield return new WaitForSeconds(onHitCueDuration);

        // Base Color
        foreach (SpriteRenderer renderer in spriteRenderer)
            renderer.color = Color.white;
    }

    public void AE_ManageObjectAtEndDeathAnim() // Animator Event
    {
        if (isEnemy)
        {
            DropPickables();
            Room myRoom = GetComponentInParent<Room>();

            if (myRoom != null)
            {
                myRoom.MyLivingEntities.Remove(this);
                myRoom.CheckLivingEntities();
            }
        }

        if (exitDeathDisablesSprite)
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        else if (exitDeathDestroys)
            DestroyMe();
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void DropPickables()
    {
        var limit = Random.Range(minDrop, maxDrop);
        for (int i = 0; i < limit; i++)
        {
            var newPickable = Instantiate(myDrops[Random.Range(0, myDrops.Length)], transform);
            newPickable.transform.parent = null;
        }
    }
}
