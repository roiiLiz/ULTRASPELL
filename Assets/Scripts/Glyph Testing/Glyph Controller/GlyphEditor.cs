using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlyphController))]
public class GlyphEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GlyphController controller = (GlyphController) target;

        GUILayout.Space(5);

        if (GUILayout.Button("Save Glyph")) {
            controller.SaveGlyph();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Clear Canvas")) {
            controller.ClearGlyph();
        }
    }
}
