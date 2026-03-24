using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chain Effect", menuName = "Hit Effects / Chain Effect")]
public class ChainEffect : OnHitEffect {
    public int ChainCount = 2;
    public float ChainRadius = 7.5f;
    public float TimeBeforeChain = 2f;

    public override void Execute(GameObject target) {
        if (target.GetComponent<ChainOrigin>() == null) {
            ChainOrigin chain = target.AddComponent<ChainOrigin>();
            chain.Initialize(
                ChainCount,
                ChainRadius,
                TimeBeforeChain,
                target.transform.position
            );
        }
    }
}
