using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimPowerUp : PowerUpBase
{
    public GameObject target;

    protected override void Update()
    {
        base.Update();

        if (cooldownReady)
        {
            cooldownReady = false;

            target = FindNearestEnemy();
            
            if (target != null)
            {
                ActivatePowerUp();
            }
        }
        else
        {
            CheckCooldown();
        }
    }

    public void ActivatePowerUp()
    {
        StartCoroutine(FireProjectiles());
    }

    private IEnumerator FireProjectiles()
    {
        for (int i = 0; i < amount.Value; i++)
        {
            // Check for a nearby enemy before firing each shot
            if (target == null || !target.activeInHierarchy)
            {
                target = FindNearestEnemy();
            }

            if (target != null)
            {
                audioSource.Play();
                FirePowerUpEntity();
            }
            else
            {
                Debug.Log("No target found, skipping shot.");
            }

            yield return new WaitForSeconds(intervalBetweenShots.Value);
        }
    }

    protected void FirePowerUpEntity()
    {
        GameObject projectile = Instantiate(powerUpEntityPrefab, playerTransform.position, Quaternion.identity, gameObject.transform);
        AutoAimProjectile autoAimProjectile = projectile.GetComponent<AutoAimProjectile>();
        if (autoAimProjectile != null)
        {
            if (target == null)
            {
                target = FindNearestEnemy();
            }
            autoAimProjectile.SetTarget(target);
        }
    }

    protected GameObject FindNearestEnemy()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerTransform.position, areaRadius.Value);
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = playerTransform.position;

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(hitCollider.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hitCollider.gameObject;
                }
            }
        }

        if (nearestEnemy != null)
        {
            Debug.Log($"Nearest enemy found: {nearestEnemy.name} at distance {minDistance}");
        }
        else
        {
            Debug.Log("No enemies found within the area radius.");
        }

        return nearestEnemy;
    }
}