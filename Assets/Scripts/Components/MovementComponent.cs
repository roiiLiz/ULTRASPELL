using System;
using System.Collections;
using UnityEngine;

public class MovementComponent : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float distanceThreshold = 0.1f;

    public void MoveInDirection(Transform transform, Vector2 direction) {
        transform.position += GetMovementDirection(transform, direction) * Time.deltaTime;
    }

    public Vector3 GetMovementDirection(Transform transform, Vector2 direction) {
        Vector3 dir = transform.right * direction.x + transform.forward * direction.y;
        dir *= speed;

        return dir;
    }

    public IEnumerator MoveToPoint(Transform transform, Vector3 position, Action onPositionReached = null) {
        while (!AtPoisiton(transform, position, distanceThreshold)) {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);

            yield return null;
        }

        onPositionReached?.Invoke();
    }

    public bool AtPoisiton(Transform transform, Vector3 position, float threshold) => Vector3.Distance(transform.position, position) > threshold;

    public void Move(VelocityComponent velocity, Vector2 movementInput) {
        Vector3 direction = GetMovementDirection(velocity.gameObject.transform, movementInput);

        velocity.AddVelocity(direction * Time.deltaTime);
    }
}
