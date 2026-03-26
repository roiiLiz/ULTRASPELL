using UnityEngine;

public class Point {
    public float x;
    public float y;

    public Point(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public Point(Vector2 vec) : this (vec.x, vec.y) { }
    public Point(Vector3 vec) : this (vec.x, vec.y) { }
}
