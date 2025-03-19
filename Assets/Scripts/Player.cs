using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Fields

    [Header("Debug Settings")]
    [SerializeField] private bool debug = true;
    [SerializeField] private bool testMode = false;

    [Header("Movement Settings")]
    [SerializeField][Range(0, 100f)] private float moveSpeed = 15f;
    [SerializeField][Range(0, 1f)] private float lerpSpeed = 0.1f; // Speed of the lerp
    [SerializeField][Range(0, 360f)] private float rotationSpeed = 200f; // Speed of the rotation
    private Vector2 moveDir = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float curHealth = 0; // Property to expose current health
    private bool healthFull;

    [Header("Collision Settings")]
    [SerializeField] private string collisionText = "Collision";

    [Header("References")]
    private GameManager gameManager;
    private EnemyManager enemyManager;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject childSprite;
    private Rigidbody2D rb2d;
    private Animator animator;

    public UnityEvent OnPlayerDeath; // Event for player death

    [SerializeField] private float directionalAlignment; // Field to show directional alignment in the Inspector

    #endregion // Fields

    #region Initialization Methods

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("Game Manager was not found.");
            }
        }

        if (enemyManager == null)
        {
            enemyManager = GameObject.FindObjectOfType<EnemyManager>();
            if (enemyManager == null)
            {
                Debug.LogError("Enemy Manager was not found.");
            }
        }

        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<Slider>();
            if (healthBar == null)
            {
                Debug.LogError("Health Bar was not found.");
            }
        }

        if (childSprite == null)
        {
            if (transform.childCount > 0)
            {
                childSprite = transform.GetChild(0).gameObject;
            }
            if (childSprite == null)
            {
                Debug.LogError("Child Sprite was not found.");
            }
        }
    }
    void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
        enemyManager = FindObjectOfType<EnemyManager>();
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.interpolation = RigidbodyInterpolation2D.Interpolate; // Enable interpolation for smoother movement
        }
        animator = GetComponent<Animator>(); // Get the Animator component
        curHealth = maxHealth;

        if (testMode && gameManager != null)
        {
            gameManager.testModeEvent.AddListener(OnTestMode);
            if (debug)
            {
                Debug.Log("Test Mode Event Added");
            }
        }
    }

    private void OnTestMode()
    {
        if (debug)  
        {
            Debug.Log("Test Mode Activated");
        }
        maxHealth = 9999;
        curHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;


    }

    #endregion // Initialization Methods

    #region Update Methods

    void FixedUpdate()
    {
        // Calculate the target velocity
        Vector2 targetVelocity = moveDir * moveSpeed;

        // Lerp towards the target velocity
        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, lerpSpeed);

        // Apply the velocity to the Rigidbody2D
        rb2d.velocity = currentVelocity;

        // Rotate the child object to face the direction of movement
        if (moveDir != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            childSprite.transform.rotation = Quaternion.RotateTowards(childSprite.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Update the ShipTurn parameter in the Animator
        if (animator != null)
        {
            directionalAlignment = GetDirectionAlignment();
            animator.SetFloat("ShipTurn", directionalAlignment);
        }
    }

    #endregion // Update Methods

    #region Health Methods

    protected void UpdateHealth(int adjustment)
    {
        if (adjustment > 0)
        {
            if (healthFull)
            {
                if (debug)
                {
                    Debug.Log("Health Full");
                }

                healthBar.gameObject.SetActive(false);
            }
            else
            {
                curHealth = Mathf.Min(curHealth + adjustment, maxHealth);

                if (debug)
                {
                    Debug.Log("Player recovered " + adjustment + " health. Current Health: " + curHealth);
                }

                healthBar.gameObject.SetActive(true);

                if (curHealth == maxHealth)
                {
                    healthFull = true;
                }
            }
        }
        else
        {
            curHealth = Mathf.Max(curHealth + adjustment, 0);

            if (debug)
            {
                Debug.Log("Player took " + adjustment + " damage. Current Health: " + curHealth);
            }

            if (curHealth == 0)
            {
                OnPlayerDeath?.Invoke(); // Trigger the player death event
            }

            healthBar.gameObject.SetActive(true);
        }

        healthBar.value = curHealth;
    }

    #endregion // Health Methods

    #region Collision Methods

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (debug)
                Debug.Log("Player");
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (debug)
                Debug.Log(collisionText);

            UpdateHealth(-1);

            enemyManager.EnemyKilled(collision.gameObject);
        }
    }

    #endregion // Collision Methods

    #region Input Methods

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    #endregion // Input Methods

    #region Utility Methods

    public float GetDirectionAlignment()
    {
        if (moveDir == Vector2.zero)
            return 0;

        Vector2 currentDir = rb2d.velocity.normalized;
        float angle = Vector2.SignedAngle(currentDir, moveDir);
        return Mathf.Clamp(angle / 180f, -1f, 1f);
    }

    #endregion // Utility Methods
}