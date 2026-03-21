using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour {
    [SerializeField] private int maxHealth = 100;
    public int CurrentHealth { get; private set; }

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

    public event Action OnDied;

    void Start() {
        CurrentHealth = maxHealth;
    }

    public void Damage(int value) {
        if (value <= 0) {
            return; 
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0, maxHealth);
        OnDamaged?.Invoke(CurrentHealth, value);

        if (CurrentHealth <= 0) {
            OnDied?.Invoke();
        }
    }

    public void Heal(int value) {
        if (value <= 0) {
            return;
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
        OnHealed?.Invoke(CurrentHealth, value);
    }
}
