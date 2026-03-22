using Unity.VisualScripting;
using UnityEngine;

public class TestProjectile : MonoBehaviour {
    [SerializeField] private OnHitEffect chainEffect;

    private HitboxComponent hitbox;
    private MovementComponent movement;

    void Awake() {
        hitbox = GetComponent<HitboxComponent>();
        movement = GetComponent<MovementComponent>();

        hitbox.AddEffect(chainEffect.Execute);
        hitbox.SetOwner(GameObject.FindGameObjectWithTag("Player"));

        hitbox.OnHitboxTriggered += () => Destroy(gameObject);
    }

    void Update() {
        movement.MoveInDirection(transform, Vector3.right);
    }
}