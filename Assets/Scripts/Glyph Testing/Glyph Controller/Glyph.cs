using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Glyph", menuName = "ULTRASPELL / Glyph", order = 0)]
public class Glyph : ScriptableObject {
    public Sprite icon;
    public List<GlyphTrainingData> trainingData = new();
    public GlyphType glyphType;
    public GlyphConfig behaviourConfig;
}

public enum GlyphType {
    Line,
    Square,
    Circle,
    Hourglass,
    Pistol,
    Shotgun,
    Star
}

public interface IGlyph {
    public void Use(GlyphData glyphData);
}