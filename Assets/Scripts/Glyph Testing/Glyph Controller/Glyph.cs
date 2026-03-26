using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Glyph", menuName = "ULTRASPELL / Glyph", order = 0)]
public class Glyph : ScriptableObject {
    public List<GlyphTrainingData> trainingData = new();
}
