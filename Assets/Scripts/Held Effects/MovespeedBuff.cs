using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Speed Buff", menuName = "Held Effects / Move Speed Buff", order = 1)]
public class MovespeedBuff : HeldEffect
{
    [SerializeField] private float movespeedMultiplier = 1.1f;
    public override void OnEquip(AttackController controller) {
        controller.GetComponent<MovementComponent>().SetSpeedMultiplier(movespeedMultiplier);
    }

    public override void OnUnequip(AttackController controller) {
        controller.GetComponent<MovementComponent>().ResetSpeedMultiplier();
    }
}
