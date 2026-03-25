using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class TrailController : MonoBehaviour {
    private ObjectPool<TrailRenderer> trailPool;

    void Start() {
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
    }

    private TrailRenderer CreateTrail() {
        GameObject go = new GameObject("Shot Trail");
        TrailRenderer trailRenderer = go.AddComponent<TrailRenderer>();

        trailRenderer.emitting = false;
        trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trailRenderer;
    }

    public IEnumerator StartTrail(Vector3 start, Vector3 end, TrailConfig config) {
        Debug.DrawRay(start, end - start, Color.yellow, 10f);

        TrailRenderer trail = trailPool.Get();

        trail.gameObject.SetActive(true);
        trail.transform.position = start;

        trail.colorGradient = config.Color;
        trail.material = config.Material;
        trail.widthCurve = config.TrailWidth;
        trail.time = config.Duration;
        trail.minVertexDistance = config.MinVertexDistance;

        yield return null;

        trail.emitting = true;

        float dist = Vector3.Distance(start, end);
        float remaining = dist;

        while (remaining > 0) {
            trail.transform.position = Vector3.Lerp(
                start,
                end,
                Mathf.Clamp01(1 - (remaining / dist))
            );

            remaining -= config.TrailSpeed * Time.deltaTime;

            yield return null;
        }

        trail.transform.position = end;

        yield return new WaitForSeconds(config.Duration);
        yield return null;

        trail.emitting = false;
        trail.gameObject.SetActive(false);

        trailPool.Release(trail);
    }
}
