using System;
using UnityEngine;

public class GlyphHittable : MonoBehaviour, IGlyphInteractable {
    [SerializeField] Glyph glyph;
    public Glyph GetGlyph => glyph;

    public static event Action<float, Vector3> OnHit;

    public void Interact(GlyphData glyphData) {
        if (glyphData.glyph == glyph) {
            OnHit?.Invoke(10f, gameObject.transform.position);           
        }
    }
}