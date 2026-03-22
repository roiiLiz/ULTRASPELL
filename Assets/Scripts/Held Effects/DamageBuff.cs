using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Multiplier Buff", menuName = "Held Effects / Damage Multiplier Buff", order = 2)]
public class DamageBuff : HeldEffect {
    [SerializeField] private float damageMultiplier = 2f;
    public override void OnEquip(SpellController spellController) {
        return;
    }

    public override void OnUnequip(SpellController spellController) {
        return;
    }
}
