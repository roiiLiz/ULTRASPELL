using UnityEngine;

public abstract class GlyphConfig : ScriptableObject, IGlyph {
    public abstract void Use(GlyphData glyphData);
}
