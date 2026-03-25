using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IDamage {
    public void DealDamage(int initialValue, IDamage initiator);
    public void HealDamage(int initialValue, IDamage initiator);
    public int CalculateDamage(int initialValue, IDamage initiator);
    public float GetOutgoingDamageMultiplier();
    public float GetIncomingDamageMultiplier();
}

public struct AttackContext {
    public AttackContext(GameObject initiator, GameObject target, int baseDamage) {
        this.initiator = initiator;
        this.target = target;
        this.baseDamage = baseDamage;
    }

    public GameObject initiator;
    public GameObject target;
    public int baseDamage;
}

public interface IDamageModifier {
    public float Modify(AttackContext context, float currentValue);
}

[Serializable]
public class DamageModifier : IDamageModifier {
    public float value;
    public float randomMin;
    public float randomMax;
    public ModifierType type;

    public float Modify(AttackContext context, float currentValue) {
        switch (type) {
            case ModifierType.FlatAdded:
                return currentValue + value;
            case ModifierType.RandomFlat:
                return currentValue + Mathf.Max(
                    UnityEngine.Random.Range(randomMin, randomMax), UnityEngine.Random.Range(randomMin, randomMax)
                );
            case ModifierType.Multiply:
                return currentValue * value;
            default:
                return currentValue;
        }
    }
}

// TODO: Write down STEP BY STEP the interaction from player input to final output

public enum ModifierType {
    FlatAdded,
    RandomFlat,
    Multiply
}

public class StatsComponent : MonoBehaviour {
}