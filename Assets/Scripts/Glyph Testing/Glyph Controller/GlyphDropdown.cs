using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class GlyphDropdown : MonoBehaviour {
    public TMP_Dropdown dropdown;
    public readonly string filePath = "Assets/Resources/Glyphs/";
    public readonly string resourcePath = "Assets/Resources/";

    public void Initialize() {
        Assert.IsNotNull(dropdown);

        dropdown.ClearOptions();

        List<string> options = new List<string>();

        System.IO.Directory.CreateDirectory("Assets/Resources/Glyphs");

        foreach (string file in System.IO.Directory.GetFiles(filePath)) {
            if (!file.EndsWith(".meta")) {
                string glyphPath = file.TrimStart(resourcePath.ToCharArray());
                glyphPath = glyphPath.TrimEnd(".asset".ToCharArray());

                Glyph glyph = Resources.Load<Glyph>(glyphPath);

                if (glyph != null) {
                    Debug.Log($"Successfully loaded the {glyph.name}!");
                    options.Add(glyph.name);
                } else {
                    Debug.LogWarning($"Could not successfully load the glyph at {glyphPath}.");
                }
            }
        }

        dropdown.AddOptions(options);
    }
}

