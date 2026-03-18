using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    [Header("Input Actions")]
    [SerializeField] private InputActionReference movementInput;
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private InputActionReference lightAttackInput;
    [SerializeField] private InputActionReference heavyAttackInput;
    [SerializeField] private InputActionReference weaponSwapInput;

    private CharacterController controller;
    private VelocityComponent velocity;
    private MovementComponent movement;
    private JumpComponent jump;
    private GravityComponent gravity;

    void Awake() {
        controller = GetComponent<CharacterController>();
        velocity = GetComponent<VelocityComponent>();
        movement = GetComponent<MovementComponent>();
        jump = GetComponent<JumpComponent>();
        gravity = GetComponent<GravityComponent>();
    }

    void Update() {
        if (controller.isGrounded && jumpInput.action.WasPressedThisFrame()) {
            jump.Jump(velocity);
        }

        movement.Move(velocity, movementInput.action.ReadValue<Vector2>());

        if (!controller.isGrounded) {
            gravity.ApplyGravity(velocity);
        }

        controller.Move(velocity.Velocity * Time.deltaTime);
        // controller.Move(finalMovementDirection);
    }
}

