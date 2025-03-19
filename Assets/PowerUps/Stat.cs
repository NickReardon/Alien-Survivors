// using System;
// using System.Collections.Generic;
// using UnityEngine;

// [System.Serializable]
// public abstract class Stat
// {
//     [SerializeField] public abstract float Value { get; }
// }

// [System.Serializable]
// public class FloatStat : Stat
// {
//     [SerializeField] private float baseValue;
//     [SerializeField] private float modifier = 1f; // Default modifier is 1
//     [SerializeField] private List<float> additionalModifiers = new List<float>(); // Use a list of floats instead

//     [SerializeField] public override float Value => CalculateFinalValue(); // Public property to access finalValue

//     public FloatStat(float baseValue)
//     {
//         this.baseValue = baseValue;
//     }

//     public void AddModifier(float modifier)
//     {
//         this.modifier += modifier;
//     }

//     public void AddAdditionalModifier(float modValue)
//     {
//         additionalModifiers.Add(modValue);
//     }

//     public void RemoveAdditionalModifier(float modValue)
//     {
//         additionalModifiers.Remove(modValue);
//     }

//     public void IncrementBaseValue(float amount)
//     {
//         baseValue += amount;
//     }

//     public void DecrementBaseValue(float amount)
//     {
//         baseValue -= amount;
//     }

//     private float CalculateFinalValue()
//     {
//         float finalValue = baseValue * modifier;
//         float fullModifier = 1;

//         foreach (var modValue in additionalModifiers)
//         {
//             fullModifier += modValue; // Apply additional modifiers as additive values
//         }
//         return finalValue;
//     }

//     public void SetBaseValue(float newBaseValue)
//     {
//         baseValue = newBaseValue;
//     }

//     public void SetModifier(float newModifier)
//     {
//         modifier = newModifier;
//     }
// }

// [System.Serializable]
// public class IntStat : Stat
// {
//     [SerializeField] private int baseValue;
//     [SerializeField] private float modifier = 1f; // Default modifier is 1
//     [SerializeField] private List<int> additionalModifiers = new List<int>(); // Use a list of ints instead
//     [SerializeField] public override float Value => CalculateFinalValue(); // Public property to access finalValue

//     public IntStat(int baseValue)
//     {
//         this.baseValue = baseValue;
//     }

//     public void AddModifier(float modifier)
//     {
//         this.modifier += modifier;
//     }

//     public void AddAdditionalModifier(int modValue)
//     {
//         additionalModifiers.Add(modValue);
//     }

//     public void RemoveAdditionalModifier(int modValue)
//     {
//         additionalModifiers.Remove(modValue);
//     }

//     public void IncrementBaseValue(int amount)
//     {
//         baseValue += amount;
//     }

//     public void DecrementBaseValue(int amount)
//     {
//         baseValue -= amount;
//     }

//     private float CalculateFinalValue()
//     {
//         float finalValue = baseValue * modifier;
//         foreach (var modValue in additionalModifiers)
//         {
//             finalValue += modValue; // Apply additional modifiers as additive values
//         }
//         return finalValue;
//     }

//     public void SetBaseValue(int newBaseValue)
//     {
//         baseValue = newBaseValue;
//     }

//     public void SetModifier(float newModifier)
//     {
//         modifier = newModifier;
//     }
// }