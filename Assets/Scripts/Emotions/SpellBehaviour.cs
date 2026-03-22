using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellBehaviour : ScriptableObject {
#region Variables

    [Header("Display Data")]
    public SpellDisplayData DisplayData;

    [Space(10)]

    [Header("Weapon Settings")]
    // public TrailConfig TrailConfig;
    public int AmmoCount = 20;
    public List<HeldEffect> MainhandHeldEffects = new List<HeldEffect>();
    public List<HeldEffect> OffhandHeldEffects = new List<HeldEffect>();

    [Space(5)]

    [Header("Light Attack Settings")]
    public float LightAttackFirerate = 2f;
    public int LightAttackAmmoCost = 1;
    public bool LightAttackIsHitscan = true;
    public int LightAttackShotCount = 1;
    public Vector3 LightAttackShotSpread = Vector3.zero;
    public TrailConfig LightTrailConfig;
    public List<OnHitEffect> LightAttackHitEffects = new List<OnHitEffect>();

    [Space(5)]

    [Header("Heavy Attack Settings")]
    public float HeavyAttackFirerate = 0.5f;
    public int HeavyAttackAmmoCost = 10;
    public bool HeavyAttackIsHitscan = true;
    public int HeavyAttackShotCount = 1;
    public Vector3 HeavyAttackShotSpread = Vector3.zero;
    public TrailConfig HeavyTrailConfig;
    public List<OnHitEffect> HeavyAttackHitEffects = new List<OnHitEffect>();

    public float GetLightAttackCooldown() => 1f / LightAttackFirerate;
    public float GetHeavyAttackCooldown() => 1f / HeavyAttackFirerate;
    public int Ammo { get; private set; }

    public event Action onAmmoDepleted;

#endregion

#region Functions

    public void EquipToMainhand(SpellController controller) {
        foreach (HeldEffect effect in MainhandHeldEffects) {
            effect.OnEquip(controller);
        }
    }
    public void EquipToOffhand(SpellController controller) {
        foreach (HeldEffect effect in OffhandHeldEffects) {
            effect.OnEquip(controller);
        }
    }
    public void UnequipFromMainhand(SpellController controller) {
        foreach (HeldEffect effect in MainhandHeldEffects) {
            effect.OnUnequip(controller);
        }
    }
    public void UnequipFromOffhand(SpellController controller) {
        foreach (HeldEffect effect in OffhandHeldEffects) {
            effect.OnUnequip(controller);
        }
    }

    public void OnHit(GameObject target, GameObject owner) {

    }
    public void OnLightAttack(GameObject owner) {
        SubtractAmmo(LightAttackAmmoCost);
    }
    public void OnHeavyAttack(GameObject owner) {
        SubtractAmmo(HeavyAttackAmmoCost);
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
}
