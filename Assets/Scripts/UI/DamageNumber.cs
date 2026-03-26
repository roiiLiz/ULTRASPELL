using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour {
    [SerializeField] AnimationCurve opacityCurve;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve heightCurve;

    Camera cam;
    TextMeshProUGUI text;
    Vector3 origin;
    float t = 0f;

    void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
        origin = transform.position;
        cam = Camera.main;
    }

    void Update() {
        text.color = new Color(1f, 1f, 1f, opacityCurve.Evaluate(t));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(t);
        transform.position = origin + (Vector3.up * heightCurve.Evaluate(t));

        t += Time.deltaTime;

        transform.forward = cam.transform.forward;
    }
}