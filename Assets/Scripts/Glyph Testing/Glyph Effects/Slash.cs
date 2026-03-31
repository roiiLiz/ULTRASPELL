using UnityEngine;

public class Slash : MonoBehaviour {
    [HideInInspector] public int damageAmount = 0;
    [SerializeField] float rotationRateMin = -30f;
    [SerializeField] float rotationRateMax = 30f;
    MovementComponent movement;

    float rotationRate;

    void Start() {
        movement = GetComponent<MovementComponent>();       
        // rotationRate = Random.Range(rotationRateMin, rotationRateMax);
        rotationRate = Random.value < 0.5f ? rotationRateMax : rotationRateMin;
    }

    void Update() {
        movement.MoveTransformInDirection(transform, transform.forward);
        transform.Rotate(Vector3.forward, rotationRate * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<IDamageable>(out var damageable)) {
            damageable.TakeDamage(damageAmount);
        }
    }
}
