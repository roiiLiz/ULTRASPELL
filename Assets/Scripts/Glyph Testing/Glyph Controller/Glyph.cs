using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Glyph", menuName = "ULTRASPELL / Glyph", order = 0)]
public class Glyph : ScriptableObject {
    public List<GlyphTrainingData> trainingData = new();
}

public class GlyphMatcher {
    public Glyph MatchGlyph(List<Vector2> points, List<Glyph> templates) {
        return null;
    }

    float GreedyCloudMatch(List<Vector2> points, List<Vector2> templatePoints) {
        return 0f;
    }

    float CloudDistance(List<Vector2> points, List<Vector2> templatePoints) {
        return 0f;
    }

    public List<Vector2> Normalize(List<Vector2> points, PointStep step = PointStep.Translated) {
        return points;
    }

    public static List<Vector2> TranslateToOrigin(List<Vector2> points) {
        List<Vector2> _points = new();
        Vector2 center = GetCenter(points);

        foreach (Vector2 point in points) {
            float x = point.x - center.x;
            float y = point.y - center.y;

            _points.Add(new Vector2(x, y));
        }

        return _points;
    }

    public static Vector2 GetCenter(List<Vector2> points) {
        float x = points.Sum(p => p.x) / points.Count;
        float y = points.Sum(p => p.y) / points.Count;

        return new Vector2(x, y);
    }

    public static List<Vector2> Scale(List<Vector2> points) {
        List<Vector2> _points = new();

        float minX = points.Select(point => point.x).Min();
        float maxX = points.Select(point => point.x).Max();
        float minY = points.Select(point => point.y).Min();
        float maxY = points.Select(point => point.y).Max();

        float scale = Mathf.Max(maxX - minX, maxY - minY);

        foreach(Vector2 point in points) {
            float x = (point.x - minX) / scale;
            float y = (point.y - minY) / scale;

            _points.Add(new Vector2(x, y));
        }

        return _points;
    }
}

public enum PointStep {
    Raw,
    Resampled,
    Scaled,
    Translated
}
