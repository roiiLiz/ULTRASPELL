using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitboxComponent : MonoBehaviour {
    [Header("Settings")]
    public LayerMask collisionLayers;
    public AttackData Attack { get; private set; }
    public GameObject Owner { get; private set; }

    public event Action<AttackData, IDamageable, Vector3> OnHitboxTriggered;

    public void SetOwner(GameObject owner) => Owner = owner;
    public void SetAttack(AttackData attack) => Attack = attack;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("HitboxComponent: Hello");
        if (collisionLayers == (collisionLayers | (1 << other.transform.gameObject.layer))) {
            // if (other.TryGetComponent<HurtboxComponent>(out HurtboxComponent hurtbox)) {
            //     hurtbox.OnHurtboxHit(OnHitEffects, Owner);
            //     OnHitboxTriggered?.Invoke();
            // }
            if (other.TryGetComponent<IHittable>(out var damageable)) {
                OnHitboxTriggered?.Invoke(Attack, other.GetComponent<IDamageable>(), other.gameObject.transform.position);
            }
        }
    }
}