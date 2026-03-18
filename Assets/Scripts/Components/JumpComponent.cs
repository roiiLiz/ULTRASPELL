using UnityEngine;

public class JumpComponent : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float jumpForce = 1.5f;

    public void Jump(VelocityComponent velocity) {
        Vector3 jump = Vector3.up * jumpForce;

        velocity.SetVelocity(new Vector3(velocity.Velocity.x, jump.y, velocity.Velocity.z));
    }
}
