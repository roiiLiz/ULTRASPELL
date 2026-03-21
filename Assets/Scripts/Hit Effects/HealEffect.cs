using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Hit Effects / Heal Effect")]
public class HealEffect : OnHitEffect {
    public int HealAmount = 0;

    public override void Execute(Collider target, GameObject owner) {
        HealthComponent health = Targeting == Targeting.Target ?
            target.GetComponent<HealthComponent>() :
            owner.GetComponent<HealthComponent>();

        if (health != null) {
            health.Heal(HealAmount);
            Debug.Log($"Healing {HealAmount} health to {health.gameObject.name}");
        }
    }
}