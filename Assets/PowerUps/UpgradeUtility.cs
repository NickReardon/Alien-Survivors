//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Reflection;

//public enum UpgradeType
//{
//    Cooldown,
//    AreaRadius,
//    BaseDamage,
//    Speed,
//    Duration,
//    Amount,
//    MaxCollisions,
//    IntervalBetweenShots,
//    Knockback,
//    LuckChance,
//    CritChance,
//    TimeBetweenHits,
//    MaxPowerUpEntitiesOnScreen,
//    SeekTarget,
//    BouncesOffCameraBounds,
//    CanPierce
//}

//public enum IncreaseType
//{
//    Flat,
//    Percentage,
//    Boolean
//}

//[System.Serializable]
//public class Upgrade
//{
//    public UpgradeType upgradeType;
//    public IncreaseType increaseType;
//    public float increaseValue;
//    public bool boolValue;
//}

//public static class UpgradeUtility
//{
//    public static void ApplyUpgrades<T>(T target, List<Upgrade> upgrades, int level) where T : PowerUpBase
//    {
//        if (level < upgrades.Count)
//        {
//            Upgrade upgrade = upgrades[level];

//            switch (upgrade.upgradeType)
//            {
//                case UpgradeType.Cooldown:
//                    ApplyIncrease(target, "cooldown", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.AreaRadius:
//                    ApplyIncrease(target, "areaRadius", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.BaseDamage:
//                    ApplyIncrease(target, "baseDamage", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.Speed:
//                    ApplyIncrease(target, "speed", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.Duration:
//                    ApplyIncrease(target, "duration", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.Amount:
//                    ApplyIncrease(target, "amount", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.MaxCollisions:
//                    ApplyIncrease(target, "maxCollisions", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.IntervalBetweenShots:
//                    ApplyIncrease(target, "intervalBetweenShots", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.Knockback:
//                    ApplyIncrease(target, "knockback", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.LuckChance:
//                    ApplyIncrease(target, "luckChance", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.CritChance:
//                    ApplyIncrease(target, "critChance", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.TimeBetweenHits:
//                    ApplyIncrease(target, "timeBetweenHits", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.MaxPowerUpEntitiesOnScreen:
//                    ApplyIncrease(target, "maxPowerUpEntitiesOnScreen", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.SeekTarget:
//                    ApplyIncrease(target, "seekTarget", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.BouncesOffCameraBounds:
//                    ApplyIncrease(target, "bouncesOffCameraBounds", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//                case UpgradeType.CanPierce:
//                    ApplyIncrease(target, "maxCollisions", IncreaseType.Flat, 1, false);
//                    ApplyIncrease(target, "canPierce", upgrade.increaseType, upgrade.increaseValue, upgrade.boolValue);
//                    break;
//            }

//            Debug.Log("Upgrade applied for level " + level + ": " + upgrade.upgradeType + " = " + (upgrade.increaseType == IncreaseType.Boolean ? upgrade.boolValue.ToString() : upgrade.increaseValue.ToString()));
//        }
//    }

//    private static void ApplyIncrease<T>(T target, string fieldName, IncreaseType increaseType, float increaseValue, bool boolValue) where T : PowerUpBase
//    {
//        var field = typeof(T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
//        if (field != null)
//        {
//            if (increaseType == IncreaseType.Boolean)
//            {
//                field.SetValue(target, boolValue);
//            }
//            else if (field.FieldType == typeof(float))
//            {
//                float baseValue = (float)field.GetValue(target);
//                float newValue = increaseType == IncreaseType.Flat ? baseValue + increaseValue : baseValue * (1 + increaseValue / 100f);
//                field.SetValue(target, newValue);
//            }
//            else if (field.FieldType == typeof(int))
//            {
//                int baseValue = (int)field.GetValue(target);
//                int newValue = increaseType == IncreaseType.Flat ? baseValue + (int)increaseValue : (int)(baseValue * (1 + increaseValue / 100f));
//                field.SetValue(target, newValue);
//            }
//            else
//            {
//                Debug.LogWarning($"Field '{fieldName}' is of unsupported type '{field.FieldType}'.");
//            }
//        }
//        else
//        {
//            Debug.LogWarning($"Field '{fieldName}' not found or not accessible on target of type '{typeof(T).Name}'.");
//        }
//    }
//}
