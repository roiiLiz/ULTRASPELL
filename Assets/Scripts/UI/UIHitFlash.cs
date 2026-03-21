using UnityEngine;

public class HitFlash : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float hitFlashDuration = 0.2f;
    [SerializeField, Range(0f, 1f)] private float hitFlashIntensity = 0.5f;

    private GameObject player;
    private float t = 0f;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            player.GetComponent<HealthComponent>().OnDamaged += FlashHit;
        }

        canvasGroup.alpha = 0f;
    }

    void OnDestroy() {
        if (player != null) {
            player.GetComponent<HealthComponent>().OnDamaged -= FlashHit;
        }
    }

    void Update() {
        t = Mathf.Clamp(t - Time.deltaTime, 0f, hitFlashDuration);
        canvasGroup.alpha = Mathf.Lerp(0f, hitFlashIntensity, t / hitFlashDuration);
    }

    private void FlashHit(int currentHealth, int damageAmount) {
        // Debug.Log("Hi");
        t = hitFlashDuration;
    }
}
