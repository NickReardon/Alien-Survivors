using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 5f; // Speed of the projectile
    [SerializeField] protected int maxCollisions = 3; // Maximum number of collisions before destruction
    [SerializeField] protected bool debug = false; // Enable debug logging
    [SerializeField] protected ParticleSystem projectileParticleSystem; // Reference to the particle system
    [SerializeField] protected float offScreenDestroyTime = 2f; // Time to wait before destroying the projectile if off-screen
    [SerializeField] protected bool seekTarget = false; // Whether the projectile should seek the target
    [SerializeField] protected GameObject target; // Target to aim at
    
    [Header("Accuracy Settings")]
    [Range(0, 1)]
    [SerializeField] protected float accuracy = 1f; // Accuracy of the projectile (1 = perfect accuracy, 0 = completely random)
    [SerializeField] protected float accuracyMultiplier = 5f; // Multiplier for the accuracy adjustment

    protected Vector2 direction; // Direction of the projectile
    protected int collisionCount = 0; // Counter for collisions
    protected float lifeSpan = 0; // Time the projectile has been alive
    protected bool isOffScreen = false; // Flag to check if the projectile is off-screen

    protected virtual void Start()
    {
        projectileParticleSystem = GetComponent<ParticleSystem>();
        if (projectileParticleSystem != null)
        {
            projectileParticleSystem.Play();
        }
        else
        {
            Debug.LogError("ParticleSystem not assigned.");
        }
    }

    protected virtual void Update()
    {
        MoveProjectile();
        lifeSpan -= Time.deltaTime;
    }

    protected virtual void MoveProjectile()
    {
        if (seekTarget && target != null)
        {
            Vector2 targetDirection = (target.transform.position - transform.position).normalized;
            direction = Vector2.Lerp(direction, targetDirection, Time.deltaTime * speed);
        }

        transform.position += (Vector3)direction * Time.deltaTime * speed;
        CheckOffScreen();
    }

    protected virtual void CheckOffScreen()
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

    protected virtual IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    protected virtual bool IsInView()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider);
    }

    protected virtual void HandleCollision(Collider2D collider)
    {
        collisionCount++;
        if (debug)
        {
            Debug.Log($"Collision Count: {collisionCount}");
            Debug.Log($"Collision with {collider.gameObject.name}");
        }

        if (collisionCount >= maxCollisions)
        {
            if (debug)
            {
                Debug.Log("Max collisions reached");
            }
            if (projectileParticleSystem != null)
            {
                StartCoroutine(StopParticlesAndDestroy());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual IEnumerator StopParticlesAndDestroy()
    {
        projectileParticleSystem.Stop();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        direction = (target.transform.position - transform.position).normalized;
        ApplyAccuracy();
        if (debug)
        {
            Debug.Log($"Target set to: {target.name}, direction: {direction}");
        }
    }

    protected void ApplyAccuracy()
    {
        float angle = UnityEngine.Random.Range(-1f, 1f) * (1f - accuracy) * accuracyMultiplier; // Adjust the multiplier as needed
        direction = Quaternion.Euler(0, 0, angle) * direction;
        if (debug)
        {
            Debug.Log($"Direction adjusted for accuracy: {direction} with angle randomness: {angle}");
        }
    }
}