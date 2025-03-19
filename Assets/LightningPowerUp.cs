using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPowerUp : PowerUpBase
{
    [Header("Lightning Settings")]
    [SerializeField] private ParticleSystem lightningEffect; // Particle system for the lightning effect
    [SerializeField] private float lightningRange = 10f; // Range to find the closest enemy
    [SerializeField] private LineRenderer lineRenderer; // LineRenderer for the lightning arc

    protected override void Start()
    {
        base.Start();
        if (lightningEffect == null)
        {
            Debug.LogError("Lightning effect particle system is not assigned.");
        }
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not assigned.");
        }
    }

    protected override void Update()
    {
        base.Update();
        CheckCooldown();
        if (cooldownReady)
        {
            ShootLightningAtClosestEnemy();
            cooldownReady = false;
        }
    }

    private void ShootLightningAtClosestEnemy()
    {
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            // Position the lightning effect at the closest enemy
            lightningEffect.transform.position = closestEnemy.transform.position;
            lightningEffect.Play();

            // Draw the lightning arc
            DrawLightningArc(playerTransform.position, closestEnemy.transform.position);
        }
    }

    private void DrawLightningArc(Vector3 start, Vector3 end)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        StartCoroutine(ClearLineRenderer());
    }

    private IEnumerator ClearLineRenderer()
    {
        yield return new WaitForSeconds(0.1f); // Adjust the duration as needed
        lineRenderer.positionCount = 0;
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = lightningRange;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
