using UnityEngine;
using UnityEngine.UI;

public class GlyphController : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] float drawingSlowdownFactor = 0.5f;
    [SerializeField] float pointDistanceThreshold = 2f;
    [SerializeField] float matchingDistanceThreshold = 0.1f;
    [SerializeField] GlyphTemplates templateCollection;

    [Space(5)]

    [Header("Dependencies")]
    [SerializeField] CanvasScaler canvasScaler;
    [SerializeField] UILineRenderer lineRenderer;
    [SerializeField] GlyphDropdown glyphDropdown;

    GlyphSaver saver = new();
    GlyphMatcher matcher = new();
    bool isDrawing = false;

    void Start() {
        lineRenderer.points.Clear();

        if (glyphDropdown != null) {
            glyphDropdown.Initialize();
        }
    }

    public bool IsDrawing() => isDrawing;
    public void ToggleGlyphDrawing() {
        isDrawing = !isDrawing;

        Time.timeScale = isDrawing ? drawingSlowdownFactor : 1f;

        Debug.Log($"Toggling drawing: {isDrawing}");
    }

    public void DrawGlyph(Vector2 mousePosition) {
        Vector2 input = ScaleInput(mousePosition);

        if (CanCreatePoint(input)) {
            lineRenderer.points.Add(input);
            lineRenderer.SetAllDirty();
        }
    }

    public void ClearGlyph() {
        if (lineRenderer.points.Count <= 0) return;

        lineRenderer.points.Clear();
        lineRenderer.SetAllDirty();
    }

    public Glyph MatchGlyph() {
        if (lineRenderer.points == null || lineRenderer.points.Count <= 0) return null;

        Glyph match = matcher.MatchGlyph(lineRenderer.points, templateCollection.templates, 64);

        return match;
    }

    bool CanCreatePoint(Vector2 position) {
        if (lineRenderer.points.Count <= 0) return true;

        return Vector2.Distance(lineRenderer.points[lineRenderer.points.Count - 1], position) > pointDistanceThreshold;
    }

    Vector2 ScaleInput(Vector2 mouseInput) {
        return new Vector2(mouseInput.x * (canvasScaler.referenceResolution.x / Screen.width), mouseInput.y * (canvasScaler.referenceResolution.y / Screen.height));
    }

    public void SaveGlyph() {
        if (saver == null || glyphDropdown == null) return;

        int index = glyphDropdown.dropdown.value;
        Glyph glyph = Resources.Load<Glyph>($"Glyphs/{glyphDropdown.dropdown.options[index].text}");

        if (glyph == null) {
            Debug.LogWarning($"Could not find glyph at path: 'Glyphs/{glyphDropdown.dropdown.options[index].text}'");
            return;
        }

        saver.SaveGlyphTrainingData(glyph, lineRenderer.points);

        ClearGlyph();
    }

    Vector2 MapMouseToScreenPoint(Vector2 mouse) {
        return new Vector2(mouse.x - (canvasScaler.referenceResolution.x / 2), mouse.y - (canvasScaler.referenceResolution.y / 2));
    }
}

