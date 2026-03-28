using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Glyph", menuName = "ULTRASPELL / Glyph", order = 0)]
public class Glyph : ScriptableObject {
    public Sprite icon;
    public List<GlyphTrainingData> trainingData = new();
    public GlyphType glyphType;
    public IGlyph glyphEffect;

    void OnValidate() {
        switch (glyphType) {
            case GlyphType.Line:
                glyphEffect = new LineGlyph();
                break;
            case GlyphType.Square:
                glyphEffect = new LineGlyph();
                break;
            case GlyphType.Circle:
                glyphEffect = new LineGlyph();
                break;
            case GlyphType.Hourglass:
                glyphEffect = new LineGlyph();
                break;
            case GlyphType.Pistol:
                glyphEffect = new LineGlyph();
                break;
            case GlyphType.Shotgun:
                glyphEffect = new LineGlyph();
                break;
            case GlyphType.Star:
                glyphEffect = new LineGlyph();
                break;
        }
    }
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
    public void Use();
}

public class LineGlyph : IGlyph {
    public void Use() {
        return;
    }
}