using System;
using System.Collections;
using UnityEngine;

public class MagicCircle : MonoBehaviour {
    [SerializeField] AnimationCurve circleSizeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] float animationDuration = 0.5f;
    [SerializeField] float rotationRate = 60f;

    // void Start() => StartCoroutine(AnimateCircle());
    void Update() => transform.Rotate(Vector3.up, rotationRate * Time.deltaTime);

    public void StartCircle(Action onCircleFinish = null) {
        StartCoroutine(AnimateCircle(onCircleFinish));
    }

    IEnumerator AnimateCircle(Action onCircleFinish) {
        float t = 0f;

        while (t < animationDuration) {
            t += Time.deltaTime;

            transform.localScale = Vector3.one * circleSizeCurve.Evaluate(t / animationDuration);

            yield return null;
        }

        onCircleFinish?.Invoke();
    }
}