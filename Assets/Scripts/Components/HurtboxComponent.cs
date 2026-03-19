using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HurtboxComponent : MonoBehaviour {
    [Header("Settings")]
    public LayerMask collisionLayers;

    public event Action onHit;

    public void OnHurtboxHit() => onHit?.Invoke();
}
