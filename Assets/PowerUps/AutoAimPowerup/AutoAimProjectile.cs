using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AutoAimProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the object

    [SerializeField] private bool seekTarget = false; // Whether the projectile should seek the target

    [SerializeField] private int maxCollisions = 3; // Maximum number of collisions before destruction
    [SerializeField] private bool ricochet = false; // Enable debug logging

    [SerializeField] private bool debug = false; // Enable debug logging

    [SerializeField] ParticleSystem projectileParticleSystem; // Reference to the particle system
    [SerializeField] ParticleSystem subEmittersModule; // Reference to the sub-emitters module

    [SerializeField] private float offScreenDestroyTime = 2f; // Time to wait before destroying the projectile if off-screen

    [SerializeField] private GameObject target; // Target to aim at

    [SerializeField] private AutoAimPowerUp parent_AutoAimPowerUp; // Reference to the parent AutoAimPowerUp

    [SerializeField] private GameObject parent; // Reference to the parent object


    private Vector2 direction; // Direction of the projectile
    private int collisionCount = 0; // Counter for collisions

    float lifeSpan = 0; // Time the projectile has been alive
    private bool isOffScreen = false; // Flag to check if the projectile is off-screen
    private bool isAlive = true; // Flag to check if the projectile is alive

private void Start()
{
    projectileParticleSystem = GetComponent<ParticleSystem>();
    if (projectileParticleSystem != null)
    {
        projectileParticleSystem.Play();
        subEmittersModule = gameObject.GetComponentInChildren<ParticleSystem>();
    }
    else
    {
        Debug.LogError("ParticleSystem not assigned.");
    }
    parent = transform.parent.gameObject;
    parent_AutoAimPowerUp = parent.GetComponent<AutoAimPowerUp>();
    SetProjectileValues();
}

public void SetProjectileValues()
{
    target = parent_AutoAimPowerUp.target;
    speed = parent_AutoAimPowerUp.speed.Value;
    seekTarget = parent_AutoAimPowerUp.seekTarget;
    maxCollisions = (int)parent_AutoAimPowerUp.maxCollisions.Value;
    ricochet = parent_AutoAimPowerUp.bouncesOffCameraBounds;
    lifeSpan = parent_AutoAimPowerUp.duration;

    // Set initial direction with slight angle randomness
    if (target != null)
    {
        direction = (target.transform.position - transform.position).normalized;
        
        // Apply slight angle randomness
        float angle = UnityEngine.Random.Range(-5f, 5f);
        direction = Quaternion.Euler(0, 0, angle) * direction;

        if (debug)
        {
            Debug.Log($"Initial direction set to: {direction} with angle randomness: {angle}");
        }
    }
    else
    {
        if (debug)
        {
            Debug.Log("No target found for the projectile.");
        }
    }
}

public void SetTarget(GameObject newTarget)
{
    target = newTarget;
    direction = (target.transform.position - transform.position).normalized;
    if (debug)
    {
        Debug.Log($"Target set to: {target.name}, direction: {direction}");
    }
}

private void Update()
{
    if (target == null || !target.activeInHierarchy)
    {
        seekTarget = false;
    }

    if(isAlive)
    {
        MoveProjectile();
    }

    lifeSpan -= Time.deltaTime;
}
private void MoveProjectile()
{
    if (seekTarget && target != null)
    {
        // Calculate the direction to the target
        Vector2 targetDirection = (target.transform.position - transform.position).normalized;
        // Lerp towards the target direction
        direction = Vector2.Lerp(direction, targetDirection, Time.deltaTime * speed);
    }

    // Move in the direction of the target or continue in the last known direction
    transform.position += (Vector3)direction * Time.deltaTime * speed;

    // Rotate to look towards movement direction
    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

    if (ricochet)
    {
        CheckScreenBoundsCollision();
    }

    CheckOffScreen();
}

    private void CheckScreenBoundsCollision()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool hitHorizontal = false;
        bool hitVertical = false;

        if (screenPoint.x <= 0)
        {
            direction.x = Mathf.Abs(direction.x); // Bounce towards the camera area
            hitHorizontal = true;
        }
        else if (screenPoint.x >= 1)
        {
            direction.x = -Mathf.Abs(direction.x); // Bounce towards the camera area
            hitHorizontal = true;
        }

        if (screenPoint.y <= 0)
        {
            direction.y = Mathf.Abs(direction.y); // Bounce towards the camera area
            hitVertical = true;
        }
        else if (screenPoint.y >= 1)
        {
            direction.y = -Mathf.Abs(direction.y); // Bounce towards the camera area
            hitVertical = true;
        }

        if (hitHorizontal || hitVertical)
        {
            if (debug)
            {
                Debug.Log("Projectile ricocheted off the screen bounds");
            }
        }
    }

    private void CheckOffScreen()
    {
        if (!IsInView())
        {
            if (!isOffScreen)
            {
                isOffScreen = true;
                StartCoroutine(DestroyAfterTime(offScreenDestroyTime));
            }
        }
        else
        {
            isOffScreen = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            HandleCollision(collider);
        }   
    }

    private void HandleCollision(Collider2D collider)
    {
        collisionCount++;
        if (debug)
        {
            Debug.Log($"Collision Count: {collisionCount}");
            Debug.Log($"Collision with {collider.gameObject.name}");
        }

        if (collisionCount >= maxCollisions)
        {
            isAlive = false;

            if (debug)
            {
                Debug.Log("Max collisions reached");
            }
            if (projectileParticleSystem != null)
            {
                if (collider != null)
                {
                    collider.enabled = false;
                }
                StartCoroutine(StopParticlesAndDestroy());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator StopParticlesAndDestroy()
    {
        projectileParticleSystem.Stop();

        transform.GetChild(0).gameObject.SetActive(true);
        
        ParticleSystem.MainModule mainModule = projectileParticleSystem.main;
        mainModule.loop = false;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }



        yield return new WaitForSeconds(3);
        Destroy(projectileParticleSystem.gameObject);
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private bool IsInView()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > -0.1f && screenPoint.x < 1.1f && screenPoint.y > -0.1f && screenPoint.y < 1.1f;
    }
}


