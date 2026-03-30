using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlyphImageController : MonoBehaviour {
    [SerializeField] AnimationCurve alphaCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] AnimationCurve sizeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] float imageDuration = 0.5f;
    [SerializeField] bool unscaledTime = true;

    RectTransform rect;
    Image image;
    NewPlayer player;
    Coroutine coroutine;
    
    void Awake() {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    void Start() {
        image.color = new Color(
            image.color.r,
            image.color.g,
            image.color.b,
            0f
        );

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();

        player.OnGlyphMatched += FlashGlyph;
    }

    void OnDestroy() {
        player.OnGlyphMatched -= FlashGlyph;
    }

    void FlashGlyph(GlyphData data) {
        if (data.glyph.icon == null) return;

        image.sprite = data.glyph.icon;

        if (data.glyph.rotateIcon) {
            float y = data.points[data.points.Count - 1].y - data.points[0].y;
            float x = data.points[data.points.Count - 1].x - data.points[0].x;

            rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(y, x) * Mathf.Rad2Deg);
        } else {
            rect.localRotation = Quaternion.identity;
        }

        if (coroutine != null) {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut() {
        float t = 0f;
        Color color = image.color;
        Vector3 scale = Vector3.one;

        while (t < imageDuration) {
            t += unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            color.a = alphaCurve.Evaluate(t / imageDuration);
            image.color = color;

            scale.x = sizeCurve.Evaluate(t / imageDuration);
            scale.y = sizeCurve.Evaluate(t / imageDuration);

            rect.localScale = scale;

            yield return null;
        }

        rect.localScale = Vector3.one;
    }
}
