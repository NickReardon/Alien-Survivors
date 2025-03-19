using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Fields

    [Header("Debug Settings")]
    [SerializeField] protected bool debug = true;

    [Header("Movement Settings")]
    [SerializeField][Range(0, 10)] protected float moveSpeed = 1f;
    [SerializeField][Range(0, 0.5f)] protected float moveSpeedFactor = 0.05f;
    protected float moveSpeedMultiplier = 1f;

    [Header("Health Settings")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float maxHealth = 100f;

    [Header("Damage Settings")]
    [SerializeField] protected float invincibilityDuration = .1f;
    private bool isInvincible = false;

    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Player player;
    [SerializeField] protected EnemyManager enemyManager;
    [SerializeField] protected CircleCollider2D circleCollider;

    [Header("Color Settings")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteRenderer outlineSpriteRenderer;
    [SerializeField] protected Color outlineColor;
    [SerializeField] protected SpriteRenderer flashSpriteRenderer;
    [SerializeField] protected Color baseColor;
    [SerializeField] protected List<Color> flashColor;
    [SerializeField] protected float flashDuration = 0.1f;

    private bool isKnockedBack = false; // Knockback state flag
    private bool isDead = false; // Death state flag

    #endregion

    #region Initialization Methods

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyManager = FindObjectOfType<EnemyManager>();
        circleCollider = GetComponent<CircleCollider2D>();

        if (debug)
            Debug.Log("Enemy Created");

        Ready();
    }

    void OnEnable()
    {
        Ready();
    }

    #endregion

    #region Update Methods

void FixedUpdate()
{
    if (!isKnockedBack && !isDead) // Skip movement logic if knocked back or dead
        {
            Movement();
        }

        if (debug)
    {
        Debug.Log($"Collider enabled: {circleCollider.enabled}");
    }
}

    protected virtual void Movement()
    {
        moveSpeedMultiplier += Time.deltaTime * moveSpeedFactor;
        Vector2 targetPosition = Vector2.MoveTowards(rb.position, player.transform.position,
            Time.fixedDeltaTime * Time.timeScale * moveSpeed * moveSpeedMultiplier);
        rb.MovePosition(targetPosition);
    }

    #endregion

    #region Support Methods

    public void Ready()
{
    moveSpeedMultiplier = 1f;
    isKnockedBack = false;
    isDead = false;
    isInvincible = false;

    if (circleCollider != null)
    {
        circleCollider.enabled = true;
    }
    else
    {
        Debug.LogError("CircleCollider2D is not assigned.");
    }

    health = maxHealth;
    ResetColors();

    if (debug)
    {
        Debug.Log("Enemy is ready.");
    }
}

    private void ResetColors()
    {

        if (outlineSpriteRenderer != null)
        {
            outlineSpriteRenderer.color = outlineColor;
        }

        if (flashSpriteRenderer != null)
        {
            flashSpriteRenderer.color = baseColor;
        }
    }

    #endregion

    #region Damage Methods

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        if (isInvincible) return;

        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (debug)
            Debug.Log($"Enemy took {damage} damage, health is now {health}");

        if (health <= 0)
        {
            if (debug)
                Debug.Log("Enemy died");
            isDead = true;
            enemyManager.EnemyKilled(gameObject);
        }
        else
        {
            StartCoroutine(HandleInvincibility());
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator HandleInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private IEnumerator FlashRed()
    {
        // spriteRenderer.color = Color.red;
        // yield return new WaitForSeconds(0.1f);
        // spriteRenderer.color = originalColor;

        foreach (Color color in flashColor)
        {
            flashSpriteRenderer.color = color;
            yield return new WaitForSeconds(flashDuration/flashColor.Count);
        }
        ResetColors();
    }

    #endregion

    #region Collision Methods

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (debug && !collision.gameObject.CompareTag("Enemy"))
            Debug.Log($"Enemy collided with {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPlayer();
        }
    }

    public void ContactPlayer()
    {
        if (debug)
            Debug.Log("Enemy contacted the player");
        // Implement logic for contacting the player
    }

    #endregion

    #region Knockback Methods

    public void Knockback(Vector2 force, float duration)
    {
        if (health <= 0)
        {
            return;
        }
        else
        {
            if (debug)
                Debug.Log($"Enemy knocked back with force {force} for duration {duration}");

            if (rb != null)
            {
                StartCoroutine(ApplyKnockback(force, duration));
            }
            else
            {
                Debug.LogError("No Rigidbody2D component found on the enemy.");
            }
        }
    }

    private IEnumerator ApplyKnockback(Vector2 force, float duration)
    {
        isKnockedBack = true;
        rb.AddForce(force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    #endregion

    #region Debug Methods

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetMoveSpeedFactor(float factor)
    {
        moveSpeedFactor = factor;
    }

    #endregion
}