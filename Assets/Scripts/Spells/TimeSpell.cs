using UnityEngine;

[CreateAssetMenu(menuName = "Spells / Create Time Spell")]
public class TimeSpell : SpellBehaviour {
    [Header("Time Spell Settings")]
    [Range(1f, 10f), Tooltip("The value is a percentage (i.e a value of 1.1f is equal to a 110% movespeed increase).")] public float mainhandMovespeedIncrease = 1.1f;

    [Space(4)]
    
    [Header("Light Attack")]
    public GameObject lightPrefab;

    [Space(4)]

    [Header("Heavy Attack")]
    public GameObject heavyPrefab;

    public override void EquipToMainhand(SpellController controller) {
        controller.GetComponent<MovementComponent>().SetSpeedMultiplier(mainhandMovespeedIncrease);
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
        controller.GetComponent<MovementComponent>().ResetSpeedMultiplier();
        return;
    }

    public override void UnequipFromOffhand(SpellController controller) {
        return;
    }
}
