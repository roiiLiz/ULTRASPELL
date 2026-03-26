using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Config", menuName = "ULTRASPELL / Weapon Config", order = 2)]
public class WeaponConfig : ScriptableObject {
    public int shotCount;
    public Vector3 spread = Vector3.zero;
    public float firerate = 1f;
    public int damageAmount = 10;
    public Glyph associatedGlyph;
}