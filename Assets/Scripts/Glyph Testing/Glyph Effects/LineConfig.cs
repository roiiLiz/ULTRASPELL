using UnityEngine;

[CreateAssetMenu(fileName = "New Line Config", menuName = "ULTRASPELL / Glyph Config / Line Config", order = 0)]
public class LineConfig : GlyphConfig {
    [SerializeField] GameObject slashPrefab;
    [SerializeField] int damageAmount;
    Camera cam;

    public override void Use(GlyphData glyphData) {

        cam = Camera.main;

        // Debug.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * 10f, Color.green, 10f);
        float y = glyphData.points[glyphData.points.Count - 1].y - glyphData.points[0].y;
        float x = glyphData.points[glyphData.points.Count - 1].x - glyphData.points[0].x;

        Vector2 center = GlyphMatcher.GetCenter(glyphData.points);
        center.Scale(new Vector2(Screen.width / 1920f, Screen.height / 1080f));
        Vector3 viewport = Camera.main.ScreenToViewportPoint(center);
        Ray ray = Camera.main.ViewportPointToRay(viewport);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.purple, 10f);

        GameObject go = GameObject.Instantiate(
            slashPrefab,
            cam.transform.position,
            cam.transform.rotation * Quaternion.Euler(ray.origin + ray.direction)
        );

        go.transform.LookAt(ray.origin + ray.direction);
        go.transform.Rotate(new Vector3(0f, 0f, Mathf.Atan2(y, x) * Mathf.Rad2Deg));

        Slash slash = go.GetComponent<Slash>();
        slash.damageAmount = damageAmount;

        float distance = GlyphMatcher.PathLength(glyphData.points);
        Debug.Log($"Distance: {distance}");
    }
}
