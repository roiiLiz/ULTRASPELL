using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Modifier", menuName = "Create Damage Modifier")]
public class DamageModifierSO : ScriptableObject {
    public List<DamageModifier> damageModifiers = new();
}
