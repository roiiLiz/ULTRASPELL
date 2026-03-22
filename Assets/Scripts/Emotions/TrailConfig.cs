using UnityEngine;

[CreateAssetMenu(fileName = "New Shot Trail Config", menuName = "Weapon System / New Shot Trail Config", order = 2)]
public class TrailConfig : ScriptableObject {
    public Material Material;
    public AnimationCurve TrailWidth;
    public float Duration = 0.5f;
    public float MinVertexDistance = 0.1f;
    public Gradient Color;

    public float MissFadeDistance = 100f;
    public float TrailSpeed = 200f;
}
