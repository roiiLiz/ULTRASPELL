using UnityEngine;

[CreateAssetMenu(menuName = "Spells / Create Time Spell")]
public class TimeSpell : SpellBehaviour {
    [Header("Time Spell Settings")]
    [Range(0f, 1f)] public float movespeedIncreasePercent = 0.1f;

    [Space(4)]
    
    [Header("Light Attack")]
    public GameObject lightPrefab;

    [Space(4)]

    [Header("Heavy Attack")]
    public GameObject heavyPrefab;

    public override void EquipToMainhand(GameObject owner) {
        return;
    }

    public override void EquipToOffhand(GameObject owner) {
        return;
    }

    public override void OnHeavyAttack(GameObject owner) {
        SubtractAmmo(HeavyAttackAmmoCost);
        return;
    }

    public override void OnHit(GameObject target, GameObject owner) {
        return;
    }

    public override void OnLightAttack(GameObject owner) {
        SubtractAmmo(LightAttackAmmoCost);
        return;
    }

    public override void UnequipFromMainhand(GameObject owner) {
        return;
    }

    public override void UnequipFromOffhand(GameObject owner) {
        return;
    }
}
