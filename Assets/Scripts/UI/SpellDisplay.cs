using System;
using TMPro;
using UnityEngine;

public class WeaponDisplay : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;

    void OnEnable() => AttackController.CurrentWeapons += DisplayWeapons;
    void OnDisable() => AttackController.CurrentWeapons -= DisplayWeapons;

    void DisplayWeapons(WeaponData current, WeaponData off) {
        text.text = $"Current Weapon: {current.name}\nOffhand Weapon: {off.name}";
    }
}
