using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Hit Effects / Damage Effect")]
public class DamageEffect : OnHitEffect {
    public int Damage = 0;

    public override void Execute(Collider target, GameObject owner) {
        target.GetComponent<HealthComponent>().Damage(Damage);
        Debug.Log($"Dealing {Damage} damage to {target.name}");
    }
}
