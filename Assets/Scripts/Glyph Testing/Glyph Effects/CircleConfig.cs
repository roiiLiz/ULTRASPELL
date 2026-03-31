using UnityEngine;

[CreateAssetMenu(fileName = "New Circle Config", menuName = "ULTRASPELL / Glyph Config / Circle Config", order = 0)]
public class CircleConfig : GlyphConfig {
    public float sphereRadius = 10f;

    public override void Use(GlyphData glyphData) {
        Vector2 center = GlyphMatcher.GetCenter(glyphData.points);
        center.Scale(new Vector2(Screen.width / 1920f, Screen.height / 1080f));
        Vector3 viewport = Camera.main.ScreenToViewportPoint(center);
        Ray ray = Camera.main.ViewportPointToRay(viewport);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 10f);

        if (Physics.SphereCast(ray, 5f, out RaycastHit hit, float.MaxValue)) {
            foreach (Collider collider in Physics.OverlapSphere(hit.point, sphereRadius)) {
                if (collider.TryGetComponent<IGlyphInteractable>(out var interactable)) {
                    interactable.Interact(glyphData);
                }
            }
        }

    }
}