using System.Collections.Generic;
using UnityEngine;

// * Debatable whether or not this should be creatable by users through the asset menu, rather than a specific scene / creator, but I digress.
// [CreateAssetMenu(fileName = "New Glyph Training Data", menuName = "ULTRASPELL / Glyph Training Data", order = 1)]
public class GlyphTrainingData : ScriptableObject {
    public List<Vector2> points = new();
}
