using TMPro;
using UnityEngine;

public class SpellDisplay : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;

    void OnEnable() => SpellController.HeldSpells += DisplaySpells;
    void OnDisable() => SpellController.HeldSpells -= DisplaySpells;

    void DisplaySpells(SpellBehaviour main, SpellBehaviour off, SpellBehaviour next) {
        text.text = $"Main Spell: {main.DisplayData.Name}\nOffhand Spell: {off.DisplayData.Name}\nNext Spell: {next.DisplayData.Name}";
    }
}
