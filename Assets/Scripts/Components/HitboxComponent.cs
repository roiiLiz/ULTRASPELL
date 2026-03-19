using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitboxComponent : MonoBehaviour {
    [Header("Settings")]
    public LayerMask collisionLayers;

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<HurtboxComponent>(out HurtboxComponent hurtbox)) {
            if (hurtbox.collisionLayers == collisionLayers) {
                hurtbox.OnHurtboxHit();
            }
        }
    }
}
