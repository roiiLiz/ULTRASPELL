using UnityEngine;

public class HitFlash : MonoBehaviour {
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float hitFlashDuration = 0.2f;
    [SerializeField, Range(0f, 1f)] float hitFlashIntensity = 0.5f;

    float t = 0f;

    void OnEnable() => Player.Damaged += FlashHit;
    void OnDisable() => Player.Damaged -= FlashHit;

    void Update() {
        t = Mathf.Clamp(t - Time.deltaTime, 0f, hitFlashDuration);
        canvasGroup.alpha = Mathf.Lerp(0f, hitFlashIntensity, t / hitFlashDuration);
    }

    private void FlashHit(int currentHealth, int damageAmount) {
        t = hitFlashDuration;
    }
}
