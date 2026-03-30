using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slash : MonoBehaviour {
    [HideInInspector] public int damageAmount = 0;
    MovementComponent movement;

    void Start() {
        movement = GetComponent<MovementComponent>();       
    }

    void Update() {
        movement.MoveTransformInDirection(transform, transform.forward);
    }

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<IDamageable>(out var damageable)) {
            damageable.TakeDamage(damageAmount);
        }
    }
}
