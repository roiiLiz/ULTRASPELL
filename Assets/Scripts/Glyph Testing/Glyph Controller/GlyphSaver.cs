using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GlyphSaver {
    int minPointCount = 10;

    public readonly string filePath = "Assets/Resources/Glyph Training Data/";
    public void SaveGlyphTrainingData(Glyph glyph, List<Vector2> points) {
        if (glyph == null || points == null || points.Count <= 0) {
            Debug.LogError("Encountered an error attempting to save glyph training data!");
            return;
        }

        if (points.Count <= minPointCount) {
            Debug.LogError("Tried saving with too few points.");
            return;
        }

        GlyphTrainingData data = ScriptableObject.CreateInstance<GlyphTrainingData>();

        data.points.AddRange(points);

        if (!System.IO.Directory.Exists(filePath)) {
            System.IO.Directory.CreateDirectory(filePath);
        } 

        int num = 1;

        foreach (string str in System.IO.Directory.GetFiles(filePath)) {
            string s = str.Trim(filePath.ToCharArray());

            Debug.Log("Evaluating substring: " + s);

            // * The string evaluates whether it ends with a '.' char because we trimmed away the string 'Assets' which means checking for the substring '.asset' will never succeed.
            if (s.StartsWith(glyph.name) && s.EndsWith(".")) {
                num++;
                Debug.Log("Increased num!");
            }
        }

        AssetDatabase.CreateAsset(data, $"{filePath}{glyph.name} Training Data {num}.asset");

        glyph.trainingData.Add(data);

        EditorUtility.SetDirty(glyph);

        AssetDatabase.SaveAssets();

        Debug.Log($"Saved training data to path: {filePath}{glyph.name}");
    }
}

