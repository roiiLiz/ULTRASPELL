using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class _Player : MonoBehaviour {
    public event Action<bool> OnRightClick;
    
    [Header("Input")]
    [SerializeField] InputActionReference leftClick;
    [SerializeField] InputActionReference rightClick;
    [SerializeField] InputActionReference mousePosition;
    [SerializeField] InputActionReference movement;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference cameraMovement;

    [Space(5)]

    [Header("Settings")]
    [SerializeField] List<GlyphCombination> combinations = new();
    [SerializeField] Transform weaponMuzzle;

    Dictionary<Glyph, WeaponConfig> weaponDictionary = new();

    CharacterController controller;
    GlyphController glyphController; 
    WeaponController weaponController;
    MovementComponent movementComponent;
    VelocityComponent velocityComponent;
    GravityComponent gravityComponent;
    CameraController cameraController;
    JumpComponent jumpComponent;

    Camera cam;

    Vector3 finalMove;

    void Awake() {
        // controller = GetComponent<CharacterController>();
        glyphController = GetComponent<GlyphController>();
        weaponController = GetComponent<WeaponController>();
        // movementComponent = GetComponent<MovementComponent>();
        // velocityComponent = GetComponent<VelocityComponent>();
        // gravityComponent = GetComponent<GravityComponent>();
        // cameraController = GetComponent<CameraController>();
        // jumpComponent = GetComponent<JumpComponent>();

        cam = Camera.main;
    }

    void Start() {
        if (combinations == null || combinations.Count <= 0) return;

        foreach (GlyphCombination combo in combinations) {
            weaponDictionary[combo.glyph] = combo.weapon;
        }
    }

    void OnLeftClick()
    {
        Debug.Log("hello");
    }

    void Update() {
        if (rightClick.action.WasPressedThisFrame()) {
            Debug.Log("Right click pressed");
            glyphController.ToggleGlyphDrawing();

            OnRightClick?.Invoke(glyphController.IsDrawing());
        }

        if (leftClick.action.ReadValue<float>() > 0f) {
            if (glyphController.IsDrawing()) {
                // Glyph drawing input
                glyphController.DrawGlyph(mousePosition.action.ReadValue<Vector2>());
            } else {
                // Weapon firing
                weaponController.FireWeapon(cam.transform, weaponMuzzle);
            }
        }

        if (leftClick.action.WasReleasedThisFrame() && glyphController.IsDrawing()) {
            glyphController.ToggleGlyphDrawing();

            // TODO: Ensure that ClearGlyph triggers the glyph for potential weapon swapping!
            glyphController.ClearGlyph();
        }
    }

    void UpdateMovement() {
        // if (controller.isGrounded && jumpInput.action.WasPressedThisFrame()) {
        //     jump.Jump(velocity);
        // }

        // if (!controller.isGrounded) {
        //     gravity.ApplyGravity(velocity);
        // }

        // finalMove = velocity.Velocity;

        // Vector3 moveDir = new Vector3(movementInput.action.ReadValue<Vector2>().x, 0f, movementInput.action.ReadValue<Vector2>().y);

        // finalMove += movement.GetMovementDirection(transform, moveDir);

        // controller.Move(finalMove * Time.deltaTime);
    }
}

[Serializable]
public class GlyphCombination {
    public WeaponConfig weapon;
    public Glyph glyph;
}
