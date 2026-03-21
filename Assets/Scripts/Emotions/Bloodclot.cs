using UnityEngine;

public class Bloodclot : MonoBehaviour {
    [SerializeField] private BloodSpell spellData;
    
    [Header("Debug")]
    [SerializeField] private bool drawDebug = true;

    private int remainingChains;
    private HitboxComponent hitbox;

    void Awake() {
        hitbox = GetComponent<HitboxComponent>();
        // hitbox.AddAction();
    }

    public void Initialize(int chainCount) {
        remainingChains = chainCount;
    }

    void OnDrawGizmos() {
        if (spellData != null && drawDebug) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, spellData.infectRange);

            foreach(Collider collider in Physics.OverlapSphere(gameObject.transform.position, spellData.infectRange)) {
                if (collider.gameObject.GetComponent<HurtboxComponent>()) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, collider.gameObject.transform.position);
                }
            }
        }
    }
}
