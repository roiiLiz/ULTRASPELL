using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Collider))]
public class HurtboxComponent : MonoBehaviour {
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
}

