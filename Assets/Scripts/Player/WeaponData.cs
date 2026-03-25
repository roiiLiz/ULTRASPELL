using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Combat System / Create New Weapon", order = 0)]
public class WeaponData : ScriptableObject {
    public AttackData lightAttack;
    public AttackData heavyAttack;
    public AttackData ultimateAttack;

    public readonly Dictionary<AttackLevel, AttackData> attacks = new();

    void OnValidate() {
        if (lightAttack != null) {
            attacks[AttackLevel.Light] = lightAttack;
        }

        if (heavyAttack != null) {
            attacks[AttackLevel.Heavy] = heavyAttack;
        }

        if (ultimateAttack != null) {
            attacks[AttackLevel.Ultimate] = ultimateAttack;
        }
    }
}
