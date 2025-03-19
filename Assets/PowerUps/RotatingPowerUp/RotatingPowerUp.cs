using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPowerUp : PowerUpBase
{
    [SerializeField]
    private GameObject powerPrefab;

    [SerializeField]
    private bool rotateClockwise = true; // New field to determine rotation direction

    protected override void Start()
    {
        base.Start();
        PowerUpsAdd();
    }

    protected void FixedUpdate()
    {
        //transform.Rotate(Vector3.forward * speed * Time.fixedDeltaTime);
        RotatePowerUps();
    }

    protected void RotatePowerUps()
    {
        while (transform.childCount < amount.Value)
        {
            PowerUpsAdd();
        }
        for (int i = 0; i < amount.Value; i++)
        {
            Transform power = transform.GetChild(i);
            float angle = 2 * Mathf.PI * i / amount.Value + Time.time * speed.Value * Mathf.Deg2Rad;
            if (rotateClockwise)
            {
                angle = -angle; // Reverse the angle for anticlockwise rotation
            }
            power.transform.position = new Vector2(transform.position.x + areaRadius.Value * Mathf.Cos(angle),
                transform.position.y + areaRadius.Value * Mathf.Sin(angle));
        }
    }

    protected void PowerUpsAdd()
    {
        // Destroy existing power-ups
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Add new power-ups
        for (int i = 0; i < amount.Value; i++)
        {
            GameObject power = Instantiate(powerPrefab, transform.position, Quaternion.identity);
            power.transform.position = new Vector2(transform.position.x + areaRadius.Value * Mathf.Cos(2 * Mathf.PI * i / amount.Value),
                transform.position.y + areaRadius.Value * Mathf.Sin(2 * Mathf.PI * i / amount.Value));
            power.transform.parent = transform; // Set the parent to keep track of instantiated power-ups
        }
    }

    public override void ApplyUpgrade()
    {
        base.ApplyUpgrade();
        PowerUpsAdd(); // Re-add power-ups to reflect the new power count if it was upgraded
    }
}