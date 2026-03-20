using UnityEngine;

[CreateAssetMenu(menuName = "Spells / Create Blood Spell")]
public class BloodSpell : SpellBehaviour {
    [Header("Blood Spell Settings")]
    public int baseSpellHealthCost = 10;

    [Space(4)]

    [Header("Light Attack")]
    public GameObject lightPrefab;
    public int bulletCount = 10;
    public Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [Range(0f, 1f)] public float healingPercent = 0.25f;
    public int minimumHealAmount = 5;

    [Space(4)]

    [Header("Heavy Attack")]
    public GameObject heavyPrefab;
    public float infectRange = 10f;
    public int chainCount = 2;

    public override void EquipToMainhand(SpellController controller) {
        return;
    }

    public override void EquipToOffhand(SpellController controller) {
        return;
    }

    // public override void OnHeavyAttack(GameObject owner) {
    //     SubtractAmmo(HeavyAttackAmmoCost);
    //     return;
    // }

    public override void OnHit(GameObject target, GameObject owner) {
        return;
    }

    // public override void OnLightAttack(GameObject owner) {
    //     SubtractAmmo(LightAttackAmmoCost);
    //     return;
    // }

    public override void UnequipFromMainhand(SpellController controller) {
        return;
    }

    public override void UnequipFromOffhand(SpellController controller) {
        return;
    }
}
