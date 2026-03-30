using TMPro;
using UnityEngine;

public class DamageNumberController : MonoBehaviour {
    [SerializeField] GameObject damageNumberPrefab;
    [SerializeField] float timeToDestroyText = 1f;
    [SerializeField] Vector3 spread = Vector3.zero;

    void OnEnable() => HealthComponent.DisplayDamage += CreateDamageNumber;
    void OnDisable() => HealthComponent.DisplayDamage -= CreateDamageNumber;

    void CreateDamageNumber(Vector3 pos, int value) {
        GameObject text = Instantiate(damageNumberPrefab, pos + new Vector3(
            UnityEngine.Random.Range(-spread.x, spread.x),
            UnityEngine.Random.Range(-spread.y, spread.y),
            UnityEngine.Random.Range(-spread.z, spread.z)
        ), Quaternion.identity);

        text.GetComponentInChildren<TextMeshProUGUI>().text = $"{value}";

        Destroy(text, timeToDestroyText);
    }
}
