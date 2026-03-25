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

    public void OnHit(List<OnHitEffect> hitEffects, GameObject hitter)
    {
        return;
    }

    void Awake() {
        _collider = GetComponent<Collider>();
    }
}


