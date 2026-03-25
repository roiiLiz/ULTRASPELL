using UnityEngine;

public abstract class HeldEffect : ScriptableObject {
    public abstract void OnEquip(AttackController controller);
    public abstract void OnUnequip(AttackController controller);
}

[CreateAssetMenu(fileName = "New Dynamic Hit Effect", menuName = "Held Effects / Add Dynamic Hit Effect", order = 2)]
public class AddHitEffect : HeldEffect {
    public OnHitEffect hitEffect;

    public override void OnEquip(AttackController controller) {
        controller.AddDynamicHitEffect(hitEffect);
    }

    public override void OnUnequip(AttackController controller) {
        controller.RemoveDynamicHitEffect(hitEffect);
    }
}
