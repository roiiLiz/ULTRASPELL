using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;

    void OnEnable() => SpellController.currentSpellInfo += DisplayAmmo;
    void OnDisable() => SpellController.currentSpellInfo -= DisplayAmmo;

    private void DisplayAmmo(SpellBehaviour spell, int ammoCount) {
        text.text = $"{spell.Name}:\n{ammoCount} / {spell.AmmoCount}";
    }
}