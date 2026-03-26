using UnityEngine;

public class GlyphController : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] float drawingSlowdownFactor = 0.5f;

    bool isDrawing = false;

    public bool IsDrawing() => isDrawing;
    public bool ToggleGlyphDrawing() {
        isDrawing = !isDrawing;

        Time.timeScale = isDrawing ? drawingSlowdownFactor : 1f;

        return isDrawing;
    }

    public void DrawGlyph(Vector2 mousePosition) {
        Debug.Log($"Mouse position: {mousePosition}");
    }
}
