using UnityEngine;

public class VelocityComponent : MonoBehaviour {
    public Vector3 Velocity { get; private set; }

    public void AddVelocity(Vector3 incomingChange) {
        Velocity += incomingChange;
    }

    public void SetVelocity(Vector3 incomingChange) {
        Velocity = incomingChange;
    }
}
