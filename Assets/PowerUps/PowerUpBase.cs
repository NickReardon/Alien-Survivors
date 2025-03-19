using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;


/// <summary>
/// PowerUpBase.cs
/// Author: [Your Name]
/// Date: [Date]
/// Description: This abstract class serves as the base for all power-up entities in the game.
/// It contains general settings, upgrade settings, and power-up settings.
/// </summary>

[System.Serializable]
public abstract class Stat
{
    [SerializeField] public abstract float Value { get; }
}

[System.Serializable]
public class FloatStat : Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private float modifier = 1f; // Default modifier is 1
    [SerializeField] private List<float> additionalModifiers = new List<float>(); // Use a list of floats instead

    [SerializeField] public override float Value => CalculateFinalValue(); // Public property to access finalValue

    public FloatStat(float baseValue)
    {
        this.baseValue = baseValue;
    }

    public void AddModifier(float modifier)
    {
        this.modifier += modifier;
    }

    public void AddAdditionalModifier(float modValue)
    {
        additionalModifiers.Add(modValue);
    }

    public void RemoveAdditionalModifier(float modValue)
    {
        additionalModifiers.Remove(modValue);
    }

    public void IncrementBaseValue(float amount)
    {
        baseValue += amount;
    }

    public void DecrementBaseValue(float amount)
    {
        baseValue -= amount;
    }

    private float CalculateFinalValue()
    {
        float finalValue = baseValue * modifier;
        float fullModifier = 1;

        foreach (var modValue in additionalModifiers)
        {
            fullModifier += modValue; // Apply additional modifiers as additive values
        }
        return finalValue;
    }

    public void SetBaseValue(float newBaseValue)
    {
        baseValue = newBaseValue;
    }

    public void SetModifier(float newModifier)
    {
        modifier = newModifier;
    }
}

[System.Serializable]
public class IntStat : Stat
{
    [SerializeField] private int baseValue;
    [SerializeField] private float modifier = 1f; // Default modifier is 1
    [SerializeField] private List<int> additionalModifiers = new List<int>(); // Use a list of ints instead
    [SerializeField] public override float Value => CalculateFinalValue(); // Public property to access finalValue

    public IntStat(int baseValue)
    {
        this.baseValue = baseValue;
    }

    public void AddModifier(float modifier)
    {
        this.modifier += modifier;
    }

    public void AddAdditionalModifier(int modValue)
    {
        additionalModifiers.Add(modValue);
    }

    public void RemoveAdditionalModifier(int modValue)
    {
        additionalModifiers.Remove(modValue);
    }

    public void IncrementBaseValue(int amount)
    {
        baseValue += amount;
    }

    public void DecrementBaseValue(int amount)
    {
        baseValue -= amount;
    }

    private float CalculateFinalValue()
    {
        float finalValue = baseValue * modifier;
        foreach (var modValue in additionalModifiers)
        {
            finalValue += modValue; // Apply additional modifiers as additive values
        }
        return finalValue;
    }

    public void SetBaseValue(int newBaseValue)
    {
        baseValue = newBaseValue;
    }

    public void SetModifier(float newModifier)
    {
        modifier = newModifier;
    }
}


#region Upgrade Classes

public enum UpgradeType
{
    Damage,
    Cooldown,
    AreaRadius,
    BaseDamage,
    Speed,
    Duration,
    Amount,
    MaxCollisions,
    Knockback,
    CritChance,
    CritMultiplier,
    LuckChance,
    IntervalBetweenShots,
    TimeBetweenHits,
    MaxPowerUpEntitiesOnScreen,
    SeekTarget,
    BouncesOffCameraBounds,

}

public enum IncreaseType
{
    Flat,
    Percentage,
    Boolean
}

[System.Serializable]
public class Upgrade
{
    public UpgradeType upgradeType;
    public IncreaseType increaseType;

    [Range(-10, 10)] public float increaseValue;

    public bool boolValue;
}

[System.Serializable]
public class UpgradeInfo
{
    public Upgrade upgrade;

    [TextArea(3, 10)] public string description;
}

#endregion // Upgrade Classes

public abstract class PowerUpBase : MonoBehaviour
{
    #region Fields


    [Header("General Settings")]
    [SerializeField] protected GameObject powerUpEntityPrefab; // Prefab for the power-up entity
    [SerializeField] public string powerUpName; // Name of the power-up
    [SerializeField][TextArea(3, 10)] public string description; // Description of the power-up
    [SerializeField] public Sprite icon; // Icon for the power-up
    [SerializeField] protected AudioClip soundEffect; // Sound effect for the power-up

    [Header("Upgrade Settings")]
    [SerializeField] public List<UpgradeInfo> upgrades; // List of upgrades available for the power-up

    [Header("Power-Up Settings")]
    [SerializeField] public FloatStat damage = new FloatStat(1f);  // Damage dealt by the power-up
    [SerializeField] public FloatStat areaRadius = new FloatStat(10f);  // Radius within which the power-up can affect targets
    [SerializeField] public FloatStat speed = new FloatStat(5f);  // Speed of the power-up entity
    [SerializeField] public IntStat amount = new IntStat(1);   // Number of projectiles or entities spawned by the power-up
    [SerializeField] public FloatStat critChance = new FloatStat(.05f);  // Chance to deal critical damage
    [SerializeField] public FloatStat critMultiplier = new FloatStat(2f);  // Multiplier for critical damage
    [SerializeField] public FloatStat knockback = new FloatStat(5f);  // Knockback force applied to targets
    [SerializeField] public FloatStat luckChance = new FloatStat(.05f);  // Chance to trigger a lucky effect

    [Header("Cooldown Settings")]
    [SerializeField] public FloatStat cooldown = new FloatStat(6f);  // Time in seconds before the power-up can be used again
    [SerializeField] protected float cooldownTimer = 0f;  // Timer to track the cooldown period
    [SerializeField] protected bool cooldownReady = true; // Flag to indicate if the power-up is ready to be used

    [Header("Projectile Settings")]
    [SerializeField] public float duration = 5f;  // Duration for which the power-up remains active
    [SerializeField] public IntStat maxCollisions = new IntStat(1);   // Maximum number of collisions before destruction
    [SerializeField] public FloatStat intervalBetweenShots = new FloatStat(1f);  // Time interval between consecutive shots

    [Header("Behavior Settings")]
    [SerializeField] public bool seekTarget = false; // Whether the power-up entity should seek the target
    [SerializeField] public bool canPierce = false; // Whether the projectiles can pierce through targets
    [SerializeField] public bool blockedByWalls = true; // Whether the projectiles are blocked by walls
    [SerializeField] public bool bouncesOffCameraBounds = false; // Whether the projectiles bounce off the camera bounds

    [Header("Tag Settings")]
    [SerializeField] protected List<string> collisionTags; // List of tags to consider for collisions

    [Header("Debug Settings")]
    [SerializeField] protected List<string> debugTextOutputs; // List of potential debug text outputs
    [SerializeField] protected bool debugOutput = false; // Control Debug.Log output

    [Header("Follow Player Settings")]
    [SerializeField] protected bool followPlayer = false; // Whether the power-up should follow the player
    [SerializeField] protected bool followPlayerRotation = false; // Whether the power-up should rotate with the player

    [Header("References")]
    [SerializeField] protected EnemyManager enemyManager;               // Reference to the EnemyManager
    [SerializeField] protected AudioSource audioSource;                  // Reference to the AudioSource
    [SerializeField] protected Transform playerTransform;               // Reference to the player's transform

    public int level = -1;    // Current level of the power-up
    public int maxLevel = 0; // Maximum level of the power-up

    #endregion // Fields

    #region Methods

    #region Upgrade Methods

    // Context Menu Methods
    [ContextMenu("Level Up")]
    public void LevelUp()
    {
        maxLevel = upgrades.Count;
        if (level < maxLevel)
        {
            ApplyUpgrade();
            level++;
        }
    }

    public virtual void ApplyUpgrade()
    {
        if (level < upgrades.Count)
        {
            var upgrade = upgrades[level].upgrade;

            switch (upgrade.upgradeType)
            {
                case UpgradeType.Damage:
                    ApplyIncrease(damage);
                    break;
                case UpgradeType.Cooldown:
                    ApplyIncrease(cooldown);
                    break;
                case UpgradeType.AreaRadius:
                    ApplyIncrease(areaRadius);
                    break;
                case UpgradeType.Speed:
                    ApplyIncrease(speed);
                    break;
                case UpgradeType.Amount:
                    ApplyIncrease(amount);
                    break;
                case UpgradeType.MaxCollisions:
                    ApplyIncrease(maxCollisions);
                    if (maxCollisions.Value > 1)
                    {
                        canPierce = true;
                    }
                    break;
                case UpgradeType.IntervalBetweenShots:
                    ApplyIncrease(intervalBetweenShots);
                    break;
                case UpgradeType.Knockback:
                    ApplyIncrease(knockback);
                    break;
                case UpgradeType.LuckChance:
                    ApplyIncrease(luckChance);
                    break;
                case UpgradeType.CritChance:
                    ApplyIncrease(critChance);
                    break;
                case UpgradeType.CritMultiplier:
                    ApplyIncrease(critMultiplier);
                    break;
                case UpgradeType.SeekTarget:
                    seekTarget = upgrade.boolValue;
                    break;
                case UpgradeType.BouncesOffCameraBounds:
                    bouncesOffCameraBounds = upgrade.boolValue;
                    break;
            }

            Debug.Log("Upgrade applied for level " + level + ": " + upgrade.upgradeType + " = " + (upgrade.increaseType == IncreaseType.Boolean ? upgrade.boolValue.ToString() : upgrade.increaseValue.ToString()));
        }
    }

    private void ApplyIncrease(FloatStat stat)
    {
        if (upgrades[level].upgrade.increaseType == IncreaseType.Flat)
        {
            stat.IncrementBaseValue((int)upgrades[level].upgrade.increaseValue);
        }
        else if (upgrades[level].upgrade.increaseType == IncreaseType.Percentage)
        {
            stat.AddModifier(upgrades[level].upgrade.increaseValue);
        }
    }

    private void ApplyIncrease(IntStat stat)
    {
        if (upgrades[level].upgrade.increaseType == IncreaseType.Flat)
        {
            stat.IncrementBaseValue((int)upgrades[level].upgrade.increaseValue);
        }
        else if (upgrades[level].upgrade.increaseType == IncreaseType.Percentage)
        {
            stat.AddModifier(upgrades[level].upgrade.increaseValue);
        }
    }


    #endregion // Upgrade Methods

    #region  Initialization Methods
    protected virtual void Start()
    {
        maxLevel = upgrades.Count;
        enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager == null && debugOutput)
        {
            Debug.LogWarning("EnemyManager not found.");
        }
        audioSource = GetComponent<AudioSource>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else if (debugOutput)
        {
            Debug.LogWarning("Player not found.");
        }
    }
    protected virtual void Update()
    {
        if (followPlayer && playerTransform != null)
        {
            FollowPlayer();
        }

        if (followPlayerRotation && playerTransform != null)
        {
            FollowPlayerRotation();
        }
    }

    public void EnablePowerUp()
    {
        gameObject.SetActive(true);
        cooldownTimer = 0;
        level = 0;
    }

    #endregion

    #region Support Methods

    protected void FollowPlayer()
    {
        transform.position = playerTransform.position;
    }

    protected void FollowPlayerRotation()
    {
        if (playerTransform.GetChild(0) != null)
        {
            transform.rotation = playerTransform.GetChild(0).rotation;
        }
        else
        {
            transform.rotation = playerTransform.rotation;
        }
    }

    protected void CheckCooldown()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownReady = false;
        }
        else
        {
            cooldownTimer = cooldown.Value;
            cooldownReady = true;
        }
    }

    protected void CheckDuration()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                //DeactivatePowerUp(); Not implemented yet
            }
        }
    }

    #endregion // Support Methods

    #region Collision Methods
    // General method for handling collisions with enemies using triggers
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (debugOutput)
        {
            Debug.Log($"OnTriggerEnter2D called with collider tag: {other.tag}");
        }
        if (collisionTags.Contains(other.tag))
        {
            if (debugOutput)
            {
                Debug.Log($"Collider tag {other.tag} is in collisionTags");
            }
            HandleCollisionWithEnemy(other.gameObject);
        }
        else if (debugOutput)
        {
            Debug.Log($"Collider tag {other.tag} is not in collisionTags");
        }
    }

    protected virtual void HandleCollisionWithEnemy(GameObject enemy)
    {
        if (debugOutput)
        {
            Debug.Log($"HandleCollisionWithEnemy called with enemy: {enemy.name}");
            Debug.Log($"{powerUpName} collided with {enemy.name}");
        }

        // Call methods on the Enemy class
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            if (debugOutput)
            {
                Debug.Log($"Enemy component found on {enemy.name}");
            }
            enemyComponent.TakeDamage(damage.Value);

            if (knockback.Value > 0)
            {
                Vector2 knockbackDirection = (enemy.transform.position - playerTransform.position).normalized;

                if (debugOutput)
                {
                    Debug.Log($"Applying knockback to {enemy.name} with direction {knockbackDirection} and value {knockback.Value}");
                }
                enemyComponent.Knockback(knockbackDirection * knockback.Value, 0.05f); // Adjust duration as needed
            }
            else if (debugOutput)
            {
                Debug.Log("Knockback value is 0 or less, no knockback applied.");
            }
        }
        else if (debugOutput)
        {
            Debug.Log($"No Enemy component found on {enemy.name}");
        }
    }

    #endregion // Collision Methods



    #endregion // Methods
}