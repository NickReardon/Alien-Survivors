using System.Collections;
using UnityEngine;

public class WithinRangeAndFire : Enemy
{
    [Header("Within Range and Fire")]
    [SerializeField] private float fireRange = 5f; // Range within which the enemy can fire
    [SerializeField] private float maxCooldown = 1f; // Maximum cooldown time before the enemy can fire again
    [SerializeField] private float fireCooldown = 0f; // Cooldown time before the enemy can fire again
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to be fired
    [SerializeField] private Transform firePoint; // Point from where the projectile will be fired
    [SerializeField] private bool canFire = true; // Flag to check if the enemy can fire

    protected void Update()
    {
        // Decrease the cooldown timer
        if (fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }
        else
        {
            canFire = true; // Allow firing when cooldown is over
        }

        Rotation(); // Rotate the enemy towards the player
    }

    protected void Rotation()
    {
        // Rotate the enemy towards the player
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    protected override void Movement()
    {
        // Check if the player is within firing range
        if (Vector2.Distance(transform.position, player.transform.position) <= fireRange)
        {
            // Calculate direction to strafe around the player
            Vector2 direction = (transform.position - player.transform.position).normalized;
            Vector2 strafeDirection = new Vector2(-direction.y, direction.x);
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)player.transform.position + strafeDirection * fireRange, Time.deltaTime * moveSpeed);

            // Fire if the enemy can fire
            if (canFire)
            {
                Fire();
            }
        }
        else
        {
            base.Movement(); // Perform base movement if the player is not within range
        }
    }

    void Fire()
    {
        // Instantiate and fire the projectile if the prefab and fire point are set
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            canFire = false; // Reset the firing flag
            fireCooldown = maxCooldown; // Reset the cooldown timer

            // Set the target for the projectile
            projectile.GetComponent<BaseProjectile>().SetTarget(player.gameObject);
        }
    }
}
