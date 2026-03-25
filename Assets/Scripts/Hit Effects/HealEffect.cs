using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Hit Effects / Heal Effect")]
public class HealEffect : OnHitEffect {
    public int HealAmount = 0;

    public override void Execute(IDamageable target) {
        target.HealDamage(HealAmount);

        Debug.Log($"Healing {HealAmount} health to {(target as HealthComponent).gameObject.name}");
    }
}