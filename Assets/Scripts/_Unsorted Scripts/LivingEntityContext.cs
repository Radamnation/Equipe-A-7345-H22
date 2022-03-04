using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField] private bool isEnemy = true;
    [SerializeField] private Pickable[] myDrops;
    [SerializeField] private int minDrop = 5;
    [SerializeField] private int maxDrop = 10;

    // SECTION - Property =========================================================
    public bool IsDead { get => currentHP.Value <= 0.0f; }
    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }


    // SECTION - Method - Unity Specific =========================================================
    private void Start()
    {
        FullHeal();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
    }


    // SECTION - Method - Context Specific =========================================================
    public void FullHeal()
    {
        currentHP.Value = maxHP.Value;
        if (onTakeDamageEvents != null)
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

        // Extend default behaviours on death here
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
            GetComponentInParent<Room>().MyLivingEntities.Remove(this);
            GetComponentInParent<Room>().CheckLivingEntities();
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
