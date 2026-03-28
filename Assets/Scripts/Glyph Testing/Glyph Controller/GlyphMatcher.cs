using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GlyphMatcher {
    float glyphMinMatchDistance = 0.05f;

    public Glyph MatchGlyph(List<Vector2> points, List<Glyph> templates, int pointCount) {
        List<Vector2> normalizedPoints = Normalize(points);
        Glyph matchedGlyph = null;
        float minDist = Mathf.Infinity;
        float dist = 0f;

        foreach (Glyph glyphTemplate in templates) {
            // Debug.Log($"Checking match for {glyphTemplate.name}");

            foreach (GlyphTrainingData templateData in glyphTemplate.trainingData) {
                List<Vector2> normalizedTemplatePoints = Normalize(templateData.points);

                dist = GreedyCloudMatch(normalizedPoints, normalizedTemplatePoints, pointCount);

                if (dist <= glyphMinMatchDistance) {
                    return null;
                }

                if (minDist > dist) {
                    minDist = dist;
                    matchedGlyph = glyphTemplate;
                }
            }
        }

        return matchedGlyph;
    }

    float GreedyCloudMatch(List<Vector2> points, List<Vector2> templatePoints, int pointCount) {
        float epislon = 0.5f;
        int step = (int) Math.Floor(Math.Pow(pointCount, 1.0f - epislon));
        float min = float.MaxValue;

        for (int i = 0; i < pointCount; i += step) {
            float firstDist = CloudDistance(points, templatePoints, pointCount, i);
            float secondDist = CloudDistance(templatePoints, points, pointCount, i);

            // Debug.Log($"First Cloud Distance: {firstDist}\nSecond Cloud Distance: {secondDist}\nMin: {min}");

            min = Mathf.Min(min, firstDist, secondDist);
        }

        return min;
    }

    float CloudDistance(List<Vector2> points, List<Vector2> templatePoints, int pointCount, int startIndex) {
        float sum = 0f;
        bool[] matchedPoints = new bool[pointCount];
        int i = startIndex;

        // * This is a do-while loop as opposed to a standard while loop because do-while loops fire at least once, which is necessary for the condition of...
        // * (i != startIndex), as we set i to the starting index before the loop.

        do {
            float min = float.MaxValue;
            int index = 0;

            for (int j = 0; j < matchedPoints.Length; j++) {
                if (i > points.Count - 1 || j > templatePoints.Count - 1) {
                    return sum;
                }

                if (!matchedPoints[j]) {
                    float dist = Vector2.Distance(points[i], templatePoints[j]);
                    if (dist < min) {
                        min = dist;
                        index = j;
                    }
                }
            }

            matchedPoints[index] = true;
            float weight = 1.0f - ((i - startIndex + pointCount) % pointCount / (1.0f * pointCount));
            sum += weight * min;
            i = (i + 1) % pointCount;
        } while (i != startIndex);

        return sum;
    }

    public List<Vector2> Normalize(List<Vector2> points) {
        List<Vector2> _points = new(points);

        _points = Resample(_points, 64);
        _points = Scale(_points);
        _points = TranslateToOrigin(_points);

        return _points;
    }

    public List<Vector2> TranslateToOrigin(List<Vector2> points) {
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

    public List<Vector2> Scale(List<Vector2> points) {
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

    public List<Vector2> Resample(List<Vector2> points, int size) {
        List<Vector2> _points = new();
        _points.Add(points[0]);

        float requiredDistBetweenPoints = PathLength(points) / (size - 1);
        float proceedDist = 0f;

        for (int i = 1; i < points.Count; i++) {
            Vector2 prev = points[i - 1];
            Vector2 curr = points[i];

            float dist = Vector2.Distance(prev, curr);

            if (proceedDist + dist >= requiredDistBetweenPoints) {
                while (proceedDist + dist >= requiredDistBetweenPoints) {
                    float t = Math.Min(Math.Max((requiredDistBetweenPoints - proceedDist) / dist, 0.0f), 1.0f);
                    if (float.IsNaN(t)) t = 0.5f;

                    float xApprox = prev.x + t * (curr.x - prev.x);
                    float yApprox = prev.y + t * (curr.y - prev.y);

                    Vector2 newPoint = new Vector2(xApprox, yApprox);

                    _points.Add(newPoint);

                    dist = proceedDist + dist - requiredDistBetweenPoints;
                    proceedDist = 0f;
                    prev = _points[_points.Count - 1];
                }

                proceedDist = dist;
            } else {
                proceedDist += dist;
            }
        }

        if (proceedDist > 0) {
            _points.Add(points[points.Count - 1]);
        }

        return _points;
    }

    float PathLength(List<Vector2> points) {
        float length = 0f;

        for (int i = 1; i < points.Count; i++) {
            Vector2 prev = points[i - 1];
            Vector2 curr = points[i];

            float dist = Vector2.Distance(prev, curr);
            if (!float.IsNaN(dist)) {
                length += dist;
            }
        }

        return length;
    }
}