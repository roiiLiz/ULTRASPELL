using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Hit Effects / Heal Effect")]
public class HealEffect : OnHitEffect {
    public int HealAmount = 0;

    public override void Execute(Collider target, GameObject owner) {
        target.GetComponent<HealthComponent>().Heal(HealAmount);
        Debug.Log($"Healing {HealAmount} health to {target.name}");
    }
}