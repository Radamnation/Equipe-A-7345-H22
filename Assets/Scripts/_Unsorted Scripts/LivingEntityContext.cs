using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LivingEntityContext : MonoBehaviour
{
    // SECTION - Field =========================================================
    [Header("Health")]
    [SerializeField] private FloatReference maxHP;
    [SerializeField] private FloatReference currentHP;

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
    }

    public void TakeDamage(float damage)
    {
        if (currentHP.Value > 0.0f)
        {
            currentHP.Value -= damage;

            Debug.Log($"{gameObject.name} has taken damage");

            StartCoroutine(TakeDamageVisualCue());

            // On Death
            if (IsDead)
            {
                OnDeathBaseHandler(); // Placed here to avoid manual storing in event
                if (onDeathEvents != null)
                    onDeathEvents.Invoke();
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
        if (exitDeathDisablesSprite)
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        else if (exitDeathDestroys)
            DestroyMe();
         
        GetComponentInParent<Room>().CheckLivingEntities();
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
