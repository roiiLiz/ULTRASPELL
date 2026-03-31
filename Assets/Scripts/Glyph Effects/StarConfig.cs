using UnityEngine;

[CreateAssetMenu(fileName = "New Star Config", menuName = "ULTRASPELL / Glyph Config / Star Config", order = 0)]
public class StarConfig : GlyphConfig {
    Camera cam;

    public override void Use(GlyphData glyphData) {
        cam = Camera.main;       

        Vector2 center = GlyphMatcher.GetCenter(glyphData.points);
        center.Scale(new Vector2(Screen.width / 1920f, Screen.height / 1080f));
        Vector3 viewport = Camera.main.ScreenToViewportPoint(center);
        Ray ray = Camera.main.ViewportPointToRay(viewport);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green, 10f);
    }
}
