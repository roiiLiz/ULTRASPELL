using UnityEngine;

public class ProjectileAttack : IAttack {
    public void Perform(Transform firingPoint, AttackData data, AttackController controller) {
        Debug.Log("Performing projectile attack!");
        if (data.projectilePrefab == null) {
            Debug.LogWarning($"{data.name} does not have a projectile prefab!");
            return;
        }

        for (int i = 0; i < data.shotCount; i++) {
            Vector3 dir = WeaponSpread.GetSpread(firingPoint.forward, data.spread);


            Projectile projectile = GameObject.Instantiate(data.projectilePrefab).GetComponent<Projectile>();

            projectile.direction = dir;
            projectile.Initialize(data, controller);
        }        
    }
}

public class Projectile : MonoBehaviour {
    public Vector3 direction;
    MovementComponent movement;
    HitboxComponent hitbox;
    AttackController controller;

    public void Initialize(AttackData atk, AttackController atkController) {
        hitbox.SetAttack(atk);
        controller = atkController;

        hitbox.OnHitboxTriggered += controller.OnHit;
    }

    void Awake() {
        movement = GetComponent<MovementComponent>();
        hitbox = GetComponent<HitboxComponent>();
    }

    void Update() {
        movement.MoveTransformInDirection(transform, direction);
    }

    void OnDestroy() {
        hitbox.OnHitboxTriggered -= controller.OnHit;
    }
}
