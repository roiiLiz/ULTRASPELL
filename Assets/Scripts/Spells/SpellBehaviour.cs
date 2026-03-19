using UnityEngine;

public abstract class SpellBehaviour : ScriptableObject {
#region Variables

    [Header("Spell Display Settings")]
    public string Name;
    [TextArea(4, 10)] public string Description;
    public Sprite Icon;

    [Header("Light Attack Settings")]
    public string LightAttackName;
    [TextArea(4, 10)] public string LightAttackDescription;
    public float LightAttackFirerate = 1f;
    public int LightAttackAmmoCost = 1;


    [Header("Heavy Attack Settings")]
    public string HeavyAttackName;
    [TextArea(4, 10)] public string HeavyAttackDescription;
    public float HeavyAttackFirerate = 0.25f;
    public int HeavyAttackAmmoCost = 1;

    public float GetLightAttackCooldown() => 1f / LightAttackFirerate;
    public float GetHeavyAttackCooldown() => 1f / HeavyAttackFirerate;

#endregion

#region Functions

    public abstract void EquipToMainhand(GameObject owner);
    public abstract void EquipToOffhand(GameObject owner);
    public abstract void UnequipFromMainhand(GameObject owner);
    public abstract void UnequipFromOffhand(GameObject owner);

    public abstract void OnHit(GameObject target, GameObject owner);
    public abstract void OnLightAttack(GameObject owner);
    public abstract void OnHeavyAttack(GameObject owner);

#endregion
}
