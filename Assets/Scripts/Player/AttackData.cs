using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Combat System / Create New Attack", order = 0)]
public class AttackData : ScriptableObject {
    public DisplayData displayData;

    [Space(5)]

    public AttackType attackType;
    [Tooltip("This value is divided evenly amongst the number of shots (i.e 20 base damage with 10 shots means each shot has a base damage of 2)")]
    public float baseDamage = 0f;
    public int shotCount = 1;
    public Vector3 spread = Vector3.zero;
    public float cooldown = 1f;
    public bool usesAmmo = true;
    public int ammoCost = 1;
    public float reloadTime = 0.25f;
    public GameObject projectilePrefab;
    public TrailConfig trailConfig;
    public IAttack attackBehaviour;

    [Space(5)]

    // public List<HeldEffect> offhandEffects = new ();
    public List<OnCastEffect> onCastEffects = new();
    public List<OnHitEffect> onHitEffects = new();
    public List<UnlockCondition> unlockConditions = new();

    public float GetFirerate() => 1f / cooldown;
    public bool IsUnlocked() {
        if (unlockConditions == null || unlockConditions.Count <= 0) return true;

        foreach (UnlockCondition condition in unlockConditions) {
            if (condition.Evaluate()) {
                continue;
            }

            return false;
        }

        return true;
    }

    void OnValidate() {
        switch (attackType) {
            case AttackType.Hitscan:
                attackBehaviour = new HitscanAttack();
                break;
            case AttackType.Projectile:
                attackBehaviour = new ProjectileAttack();
                break;
            case AttackType.Ability:
                attackBehaviour = new AbilityAttack();
                break;
            default:
                break;
        }
    }
}
