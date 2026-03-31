using UnityEngine;
using System.Collections.Generic;

public interface IGlyphInteractable {
    public Glyph GetGlyph { get; }
    public void Interact(GlyphData glyphData);
}

[System.Serializable]
public struct GlyphData {
    public Glyph glyph;
    public List<Vector2> points;

    public GlyphData(Glyph glyph, List<Vector2> points) {
        this.glyph = glyph;
        this.points = points;
    }
}

