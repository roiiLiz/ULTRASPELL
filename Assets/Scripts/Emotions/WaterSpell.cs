using UnityEngine;

[CreateAssetMenu(menuName = "Spells / Create Water Spell")]
public class WaterSpell : SpellBehaviour {
    public override void EquipToMainhand(SpellController controller) {
        return;
    }

    public override void EquipToOffhand(SpellController controller) {
        return;
    }

    // public override void OnHeavyAttack(GameObject owner)
    // {
    //     throw new System.NotImplementedException();
    // }

    public override void OnHit(GameObject target, GameObject owner) {
        return;
    }

    // public override void OnLightAttack(GameObject owner)
    // {
    //     throw new System.NotImplementedException();
    // }

    public override void UnequipFromMainhand(SpellController controller) {
        return;
    }

    public override void UnequipFromOffhand(SpellController controller) {
        return;
    }
}