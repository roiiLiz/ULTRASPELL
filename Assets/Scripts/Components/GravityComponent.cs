using UnityEngine;

public class GravityComponent : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float gravityForce = -9.81f;
    [SerializeField] private float fallingMultiplier = 2f;

    public void ApplyGravity(VelocityComponent velocity) {
        Vector3 gravity = Vector3.up * gravityForce * Time.deltaTime;

        if (velocity.Velocity.y <= 0f) {
            gravity *= fallingMultiplier;
        }

        velocity.AddVelocity(gravity);
    }
}
