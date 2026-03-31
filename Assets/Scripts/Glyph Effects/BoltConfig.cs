using UnityEngine;

[CreateAssetMenu(fileName = "New Bolt Config", menuName = "ULTRASPELL / Glyph Config / Bolt Config", order = 0)]
public class BoltConfig : GlyphConfig {
    [SerializeField] GameObject boltPrefab;
    Camera cam;

    public override void Use(GlyphData glyphData) {
        cam = Camera.main;

        Vector2 point = glyphData.points[glyphData.points.Count - 1];
        point.Scale(new Vector2(Screen.width / 1920f, Screen.height / 1080f));
        Vector3 viewport = cam.ScreenToViewportPoint(point);
        Ray ray = cam.ViewportPointToRay(viewport);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.yellow, 10f);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue)) {
            // Debug.DrawLine(cam.transform.position, hit.point, Color.yellowGreen, 10f);
            GameObject bolt = Instantiate(boltPrefab, hit.point, Quaternion.identity);
        }
    }
}
