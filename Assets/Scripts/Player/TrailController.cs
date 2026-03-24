using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TrailController : MonoBehaviour {
    private ObjectPool<TrailRenderer> lightTrailPool;
    private ObjectPool<TrailRenderer> heavyTrailPool;

    private SpellBehaviour currentSpell;

    void OnEnable() => SpellController.HeldSpells += UpdateCurrentSpell;
    void OnDisable() => SpellController.HeldSpells -= UpdateCurrentSpell;

    private void UpdateCurrentSpell(SpellBehaviour main, SpellBehaviour offhand, SpellBehaviour next) {
        currentSpell = main;
    }

    void Start() {
        if (currentSpell != null) {
            lightTrailPool = new ObjectPool<TrailRenderer>(() => CreateTrail(currentSpell.LightAttack.trailConfig));
            heavyTrailPool = new ObjectPool<TrailRenderer>(() => CreateTrail(currentSpell.HeavyAttack.trailConfig));
        }
    }

    private TrailRenderer CreateTrail(TrailConfig config) {
        GameObject go = new GameObject("Shot Trail");
        TrailRenderer trailRenderer = go.AddComponent<TrailRenderer>();
        trailRenderer.colorGradient = config.Color;
        trailRenderer.material = config.Material;
        trailRenderer.widthCurve = config.TrailWidth;
        trailRenderer.time = config.Duration;
        trailRenderer.minVertexDistance = config.MinVertexDistance;

        trailRenderer.emitting = false;
        trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trailRenderer;
    }

    public IEnumerator StartTrail(Vector3 start, Vector3 end, TrailConfig config, AttackType attackType) {
        Debug.DrawRay(start, end - start, Color.yellow, 10f);

        TrailRenderer trail = attackType == AttackType.Light ? lightTrailPool.Get() : heavyTrailPool.Get();
        trail.gameObject.SetActive(true);
        trail.transform.position = start;
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

        switch (attackType)
        {
            case AttackType.Light:
                lightTrailPool.Release(trail);
                break;
            case AttackType.Heavy:
                heavyTrailPool.Release(trail);
                break;
            default:
                break;
        }
    }
}
