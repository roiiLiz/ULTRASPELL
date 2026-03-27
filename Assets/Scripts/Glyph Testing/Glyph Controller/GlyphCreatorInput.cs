using UnityEngine;
using UnityEngine.InputSystem;

public class GlyphCreatorInput : MonoBehaviour
{
    [SerializeField] InputActionReference leftClick;
    [SerializeField] InputActionReference rightClick;
    [SerializeField] InputActionReference mousePosition;

    GlyphController controller;

    void Awake() {
        controller = GetComponent<GlyphController>();
    }

    void Update() {
        if (leftClick.action.ReadValue<float>() > 0f) {
            controller.DrawGlyph(mousePosition.action.ReadValue<Vector2>());
        }

        if (leftClick.action.WasReleasedThisFrame()) {
            Glyph glyph = controller.MatchGlyph();
            Debug.Log($"Matched glyph: {(glyph == null ? "None" : glyph.name)}.");
        }

        if (rightClick.action.WasReleasedThisFrame()) {
            controller.ClearGlyph();
        }
    }
}
