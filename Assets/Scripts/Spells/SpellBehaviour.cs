using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SpellBehaviour : ScriptableObject {
#region Variables

    [Header("Display Data")]
    public SpellDisplayData DisplayData;

    [Space(4)]

    [Header("Weapon Settings")]
    public int AmmoCount = 20;
    public float LightAttackFirerate = 1f;
    public int LightAttackAmmoCost = 1;

    public float HeavyAttackFirerate = 0.25f;
    public int HeavyAttackAmmoCost = 10;

    public float GetLightAttackCooldown() => 1f / LightAttackFirerate;
    public float GetHeavyAttackCooldown() => 1f / HeavyAttackFirerate;
    public int Ammo { get; private set; }

    public event Action onAmmoDepleted;

#endregion

#region Functions

    public abstract void EquipToMainhand(SpellController controller);
    public abstract void EquipToOffhand(SpellController controller);
    public abstract void UnequipFromMainhand(SpellController controller);
    public abstract void UnequipFromOffhand(SpellController controller);

    public abstract void OnHit(GameObject target, GameObject owner);
    public virtual void OnLightAttack(GameObject owner) {
        SubtractAmmo(LightAttackAmmoCost);
    }
    public virtual void OnHeavyAttack(GameObject owner) {
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
