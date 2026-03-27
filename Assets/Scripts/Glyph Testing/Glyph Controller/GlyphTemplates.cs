using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Glyph Templates", menuName = "ULTRASPELL / Glyph Template Collection", order = 1)]
public class GlyphTemplates : ScriptableObject {
    public List<Glyph> templates = new();
}
