using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Glyph", menuName = "ULTRASPELL / Glyph", order = 0)]
public class Glyph : ScriptableObject {
    public Sprite icon;
    public List<GlyphTrainingData> trainingData = new();
    public bool toggleDrawingOnMatch = true;
    public bool rotateIcon = false;
    public GlyphConfig behaviourConfig;
}

public interface IGlyph {
    public void Use(GlyphData glyphData);
}