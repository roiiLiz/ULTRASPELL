using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayer : MonoBehaviour {
    public event Action<GlyphData> OnGlyphMatched;

    [Header("Input Actions")]
    [SerializeField] InputActionReference leftClick;
    [SerializeField] InputActionReference rightClick;
    [SerializeField] InputActionReference mousePosition;
    [SerializeField] InputActionReference cameraMovement;
    [SerializeField] InputActionReference movement;
    [SerializeField] InputActionReference jump;

    [Space(5)]
    [Header("Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] MovementComponent movementComponent;
    [SerializeField] CameraController cameraController;
    [SerializeField] JumpComponent jumpComponent;
    [SerializeField] GravityComponent gravityComponent;
    [SerializeField] VelocityComponent velocityComponent;
    [SerializeField] GlyphController glyphController;

    [Space(5)]

    [Header("Settings")]
    [SerializeField] float delayBeforeCameraControl = 0.2f;

    bool allowCameraControl = true;
    Vector3 finalMovementDirection;
    Camera cam;


    void Awake() {
        cam = Camera.main;
    }

    void Start() {
        cameraController.CaptureMouse();
    }

    void Update() {
        UpdateMovement();
        UpdateCamera();
        UpdateInteraction();
    }

    void UpdateMovement() {
        if (characterController.isGrounded && jump.action.WasPressedThisFrame()) {
            jumpComponent.Jump(velocityComponent);
        }

        if (!characterController.isGrounded) {
            gravityComponent.ApplyGravity(velocityComponent);
        }

        Vector2 movementInput = movement.action.ReadValue<Vector2>();
        Vector3 horizontalMovementDirection = new Vector3(movementInput.x, 0f, movementInput.y);

        finalMovementDirection = velocityComponent.Velocity;

        finalMovementDirection += movementComponent.GetMovementDirection(transform, horizontalMovementDirection);

        characterController.Move(finalMovementDirection * Time.deltaTime);
    }

    void UpdateCamera() {
        if (!glyphController.IsDrawing() && allowCameraControl) {
            cameraController.RotateCamera(transform, cam, cameraMovement.action.ReadValue<Vector2>());
        }
    }

    void UpdateInteraction() {
        if (rightClick.action.WasPressedThisFrame()) {
            ToggleDrawing();
        }

        if (glyphController.IsDrawing()) {
            if (leftClick.action.ReadValue<float>() > 0f) {
                glyphController.DrawGlyph(mousePosition.action.ReadValue<Vector2>());
            }

            if (leftClick.action.WasReleasedThisFrame()) {
                GlyphData glyphData = glyphController.MatchGlyph();
                if (glyphData.glyph != null) {
                    OnGlyphMatched?.Invoke(glyphData);

                    glyphData.glyph.behaviourConfig?.Use(glyphData);
                }

                glyphController.ClearGlyph();

                if (glyphData.glyph != null) {
                    if (glyphData.glyph.toggleDrawingOnMatch) {
                        StartCoroutine(PauseCameraControl());
                        ToggleDrawing();
                    }
                }

                Debug.Log($"Glyph matched: {(glyphData.glyph == null ? "None" : glyphData.glyph.name)}."); }
        } else {
            if (leftClick.action.ReadValue<float>() > 0f) {
                return;
            }
        }
    }

    void ToggleDrawing() {
        glyphController.ToggleGlyphDrawing();
        cameraController.ToggleMouse();
    }

    IEnumerator PauseCameraControl() {
        allowCameraControl = false;

        yield return new WaitForSeconds(delayBeforeCameraControl);

        allowCameraControl = true;
    }
}
