using UnityEngine;

public class Projectile : MonoBehaviour {
    [HideInInspector] public Vector3 direction;
    MovementComponent movement;
    HitboxComponent hitbox;
    AttackController controller;

    public void Initialize(AttackData atk, AttackController atkController) {
        hitbox.SetAttack(atk);
        controller = atkController;

        Physics.IgnoreCollision(hitbox.GetComponent<Collider>(), atkController.gameObject.GetComponent<Collider>());

        hitbox.OnHitboxTriggered += OnHit;
    }

    void OnHit(AttackData data, IDamageable target, Vector3 damagePoint) {
        controller.OnHit(data, target, damagePoint);
        Destroy(gameObject);
    }

    void Awake() {
        movement = GetComponent<MovementComponent>();
        hitbox = GetComponent<HitboxComponent>();
    }

    void Update() {
        movement.MoveTransformInDirection(transform, direction);
    }

    void OnDestroy() {
        hitbox.OnHitboxTriggered -= OnHit;
    }
}
