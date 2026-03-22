using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitboxComponent : MonoBehaviour {
    [Header("Settings")]
    public LayerMask collisionLayers;

    public GameObject Owner { get; private set; }
    public readonly List<HitEffect> OnHitEffects = new();

    public event Action OnHitboxTriggered;

    public void AddEffect(HitEffect action) => OnHitEffects.Add(action);
    public void RemoveEffect(HitEffect action) {
        if (OnHitEffects == null || OnHitEffects.Count <= 0 || !OnHitEffects.Contains(action)) {
            return;
        }

        OnHitEffects.Remove(action);
    }

    public void ResetEffects() => OnHitEffects.Clear();

    public void SetOwner(GameObject owner) => Owner = owner;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("HitboxComponent: Hello");
        if (collisionLayers == (collisionLayers | (1 << other.transform.gameObject.layer))) {
            if (other.TryGetComponent<HurtboxComponent>(out HurtboxComponent hurtbox)) {
                hurtbox.OnHurtboxHit(OnHitEffects, Owner);
                OnHitboxTriggered?.Invoke();
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
