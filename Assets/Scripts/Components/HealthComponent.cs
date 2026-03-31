using System;
using UnityEngine;

public interface IDamageable {
    public void TakeDamage(int value);
    public void HealDamage(int value);

    public void SetDamageMultiplier(float value);
    public void ResetDamageMultiplier();
}

public class HealthComponent : MonoBehaviour, IDamageable {
    [SerializeField] private int maxHealth = 100;
    public int CurrentHealth { get; private set; }
    public float DamageMultiplier { get; private set; }

    [SerializeField] float noHitNotificationInterval = 1f;
    float timeNotHit = 0f;

    /// <summary>
    /// Emits the health component's current and max health values, respectively.
    /// </summary>
    public event Action<int, int> DisplayHealth;

    /// <summary>
    /// When damaged, this event emits the health component's current health and damage value recieved.
    /// </summary>
    public event Action<int, int> OnDamaged;

    /// <summary>
    /// When healed, this event emits the health component's current health and healing value recieved.
    /// </summary>
    public event Action<int, int> OnHealed;

    public event Action<float> NoHitDuration;
    public event Action OnDied;

    public static event Action<Vector3, int> DisplayDamage;

    void Start() {
        CurrentHealth = maxHealth;
        ResetDamageMultiplier();
    }

    void Update() {
        timeNotHit += Time.deltaTime;

        NoHitDuration?.Invoke(timeNotHit);
    }

    public void SetDamageMultiplier(float value) => DamageMultiplier = value;
    public void ResetDamageMultiplier() => DamageMultiplier = 1f;

    public void TakeDamage(int value) {
        if (value <= 0) {
            return; 
        }

        timeNotHit = 0f;

        CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0, maxHealth);
        OnDamaged?.Invoke(CurrentHealth, value);
        DisplayDamage?.Invoke(transform.position, value);

        if (CurrentHealth <= 0) {
            OnDied?.Invoke();
        }
    }

    public void HealDamage(int value) {
        if (value <= 0) {
            return;
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
        OnHealed?.Invoke(CurrentHealth, value);
    }
}

