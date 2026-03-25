using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aspect", menuName = "Aspects / Create New Aspect", order = 0)]
public class SpellBehaviour : ScriptableObject {
#region Variables

    [Header("Weapon Settings")]
    public int AmmoCount = 20;
    public List<HeldEffect> OffhandHeldEffects = new List<HeldEffect>();

    [Space(5)]
    public AttackInfo LightAttack;
    public AttackInfo HeavyAttack;

    public int Ammo { get; private set; }

    public event Action onAmmoDepleted;

#endregion

#region Functions

    public void EquipToMainhand(SpellController controller) {
        // foreach (HeldEffect effect in MainhandHeldEffects) {
        //     effect.OnEquip(controller);
        // }
    }
    public void EquipToOffhand(SpellController controller) {
        foreach (HeldEffect effect in OffhandHeldEffects) {
            effect.OnEquip(controller);
        }
    }
    public void UnequipFromMainhand(SpellController controller) {
        // foreach (HeldEffect effect in MainhandHeldEffects) {
        //     effect.OnUnequip(controller);
        // }
    }
    public void UnequipFromOffhand(SpellController controller) {
        foreach (HeldEffect effect in OffhandHeldEffects) {
            effect.OnUnequip(controller);
        }
    }
    public void OnLightAttack(GameObject owner) {
        // SubtractAmmo(LightAttackAmmoCost);
        SubtractAmmo(LightAttack.ammoCost);
    }
    public void OnHeavyAttack(GameObject owner) {
        // SubtractAmmo(HeavyAttackAmmoCost);
        SubtractAmmo(HeavyAttack.ammoCost);
    }

    public void SubtractAmmo(int amount) {
        Ammo -= amount;

        Debug.Log($"Ammo amount: {Ammo}");

        if (Ammo <= 0) {
            onAmmoDepleted?.Invoke();
        }
    }

    public void SetAmmo(int amount) => Ammo = amount;
    public void ReplenishAmmo() => Ammo = AmmoCount;

    public void BindToUnlocks(GameObject owner) {
        foreach (UnlockCondition condition in LightAttack.unlockConditions) {
            condition.BindEvaluation(owner);
        }
        
        foreach (UnlockCondition condition in HeavyAttack.unlockConditions) {
            condition.BindEvaluation(owner);
        }
    }

    public void UnbindToUnlocks(GameObject owner) {
        foreach (UnlockCondition condition in LightAttack.unlockConditions) {
            condition.UnbindEvaluation(owner);
        }

        foreach (UnlockCondition condition in HeavyAttack.unlockConditions) {
            condition.UnbindEvaluation(owner);
        }
    }

#endregion
}

[Serializable]
public struct SpellDisplayData {
    [Header("Spell Display Settings")]
    public string Name;
    [TextArea(4, 10)] public string Description;
    public Sprite Icon;

    [Space(4)]

    [Header("Light Attack Settings")]
    public string LightAttackName;
    [TextArea(4, 10)] public string LightAttackDescription;

    [Space(4)]

    [Header("Heavy Attack Settings")]
    public string HeavyAttackName;
    [TextArea(4, 10)] public string HeavyAttackDescription;

    [Space(4)]

    [Header("Ultimate Settings")]
    public string UltimateName;
    [TextArea(4, 10)] public string UltimateDescription;
}

[Serializable]
public class AttackInfo {
    public SpellDisplayData displayData;
    public AttackType attackType;
    // public ShotType shotType;
    public float firerate = 1f;
    [Tooltip("Attack cooldown in seconds.")] public float cooldown = 1f;
    public int ammoCost = 1;
    // public bool isHitscan = true;
    public int shotCount = 1;
    public Vector3 spread = Vector3.zero;
    public TrailConfig trailConfig;
    public List<OnCastEffect> castEffects = new();
    public List<OnHitEffect> hitEffects = new();
    public List<UnlockCondition> unlockConditions = new();


    public bool Unlocked() {
        if (unlockConditions == null || unlockConditions.Count <= 0) return true;

        foreach (UnlockCondition condition in unlockConditions) {
            if (condition.Evaluate()) {
                continue;
            } 

            return false;
        }

        return true;
    }

    public void UpdateConditionals(GameObject owner) {
        for (int i = unlockConditions.Count - 1; i >= 0; i--) {
            if (unlockConditions[i].Evaluate()) {
                unlockConditions[i].UnbindEvaluation(owner);
            }
        }
    }

    public float GetFirerate() => 1f / firerate;
}


public abstract class OnCastEffect : ScriptableObject {
    public abstract void Execute(GameObject owner);
}


