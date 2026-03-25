using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chain Effect", menuName = "Hit Effects / Chain Effect")]
public class ChainEffect : OnHitEffect {
    public int ChainCount = 2;
    public float ChainRadius = 7.5f;
    public float TimeBeforeChain = 2f;

    public override void Execute(IDamageable target) {
        GameObject go = (target as HealthComponent).gameObject;
        if (go.GetComponent<ChainOrigin>() == null)
        {
            ChainOrigin chain = go.AddComponent<ChainOrigin>();
            chain.Initialize(
                ChainCount,
                ChainRadius,
                TimeBeforeChain,
                go.transform.position
            );
        }
    }
}
