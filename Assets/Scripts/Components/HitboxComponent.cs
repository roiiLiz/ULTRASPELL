using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitboxComponent : MonoBehaviour {
    [Header("Settings")]
    public LayerMask collisionLayers;

    public GameObject Owner { get; private set; }
    public readonly List<HitEffect> OnHitEffects = new();

    public void AddEffect(HitEffect action) => OnHitEffects.Add(action);
    public void RemoveEffect(HitEffect action) {
        if (OnHitEffects == null || OnHitEffects.Count <= 0 || !OnHitEffects.Contains(action)) {
            return;
        }

        OnHitEffects.Remove(action);
    }

    public void SetOwner(GameObject owner) => Owner = owner;

    private void OnTriggerEnter(Collider other) {
        if (collisionLayers == (collisionLayers | (1 << other.transform.gameObject.layer))) {
            if (other.TryGetComponent<HurtboxComponent>(out HurtboxComponent hurtbox)) {
                hurtbox.OnHurtboxHit(OnHitEffects, Owner);
            }
        }
    }
}

/// <summary>
/// Describes an arbitrary hit effect done to a target by an owner.
/// </summary>
/// <param name="target"></param>
/// <param name="owner"></param>
public delegate void HitEffect(Collider target, GameObject owner);
