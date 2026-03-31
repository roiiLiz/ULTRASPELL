using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Bolt : MonoBehaviour {
    [SerializeField] float beamDuration = 3f;
    [SerializeField] VisualEffect beamParticle;
    [SerializeField] MagicCircle innerestCircle;
    [SerializeField] MagicCircle innerCircle;
    [SerializeField] MagicCircle outerCircle;

    void Start() {
        innerestCircle.StartCircle();
        innerCircle.StartCircle();
        outerCircle.StartCircle(SpawnBeam);
    }

    void SpawnBeam() {
        beamParticle.Play();
        StartCoroutine(OnBeamFinish(() => Destroy(gameObject)));
    }

    IEnumerator OnBeamFinish(Action onBeamFinish) {
        float t = 0f;

        while (t < beamDuration) {
            t += Time.deltaTime;
            yield return null;
        }

        onBeamFinish.Invoke();
    }
}
