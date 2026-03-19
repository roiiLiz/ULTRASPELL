using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float cameraSens = 10.0f;
    [SerializeField] private float maxLookAngle = 45f;
    [SerializeField] private float minLookAngle = -45f;

    float xRot = 0.0f;

    public void CaptureMouse() => Cursor.lockState = CursorLockMode.Locked;

    public void RotateCamera(Transform transform, Camera camera, Vector2 lookDir) {
        Vector2 mouse = new Vector2(lookDir.x * cameraSens * Time.deltaTime, lookDir.y * cameraSens * Time.deltaTime);

        xRot = Mathf.Clamp(xRot - mouse.y, minLookAngle, maxLookAngle);

        camera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        transform.Rotate(Vector3.up * (lookDir.x * cameraSens * Time.deltaTime));
    }
}

