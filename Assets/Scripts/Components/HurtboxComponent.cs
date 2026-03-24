using System.Collections.Generic;
using UnityEngine;

public interface IHittable {
    public void OnHit(List<OnHitEffect> hitEffects, GameObject hitter);
}

[RequireComponent(typeof(Collider))]
public class HurtboxComponent : MonoBehaviour, IHittable {
    [Header("Settings")]
    public LayerMask collisionLayers;
    private Collider _collider;

    void Awake() {
        _collider = GetComponent<Collider>();
    }

    public void OnHurtboxHit(List<HitEffect> effects, GameObject owner) {
        if (effects == null || effects.Count <= 0) {
            return;
        }

        foreach (HitEffect effect in effects) {
            effect?.Invoke(_collider, owner);
        }
    }

    public void _OnHurtboxHit(List<OnHitEffect> effects, GameObject owner) {
        if (effects == null || effects.Count <= 0) {
            return;
        }

        foreach (OnHitEffect effect in effects) {
            // effect?.Execute(_collider, owner);
        }
    }

    public void OnHit(List<OnHitEffect> hitEffects, GameObject hitter) {
        if (hitEffects == null || hitEffects.Count <= 0) return;
        
        foreach (OnHitEffect effect in hitEffects) {
            if (effect == null) continue;

            effect.Execute(effect.Targeting == Targeting.Target ? gameObject : hitter);
        }
    }
}


