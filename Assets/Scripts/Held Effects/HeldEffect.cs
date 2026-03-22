using UnityEngine;

public abstract class HeldEffect : ScriptableObject {
    public abstract void OnEquip(SpellController spellController);
    public abstract void OnUnequip(SpellController spellController);
}
