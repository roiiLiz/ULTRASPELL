using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

#region Variables

    [Header("Input Actions")]
    [SerializeField] private InputActionReference movementInput;
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private InputActionReference lookInput;
    [SerializeField] private InputActionReference lightAttackInput;
    [SerializeField] private InputActionReference heavyAttackInput;
    [SerializeField] private InputActionReference weaponSwapInput;

    private CharacterController controller;
    private VelocityComponent velocity;
    private MovementComponent movement;
    private JumpComponent jump;
    private GravityComponent gravity;
    private CameraController camComponent;
    private HurtboxComponent hurtbox;
    private Camera cam;

    Vector3 finalMove;

#endregion

#region MonoBehaviour Methods
    void Awake() {
        controller = GetComponent<CharacterController>();
        velocity = GetComponent<VelocityComponent>();
        movement = GetComponent<MovementComponent>();
        jump = GetComponent<JumpComponent>();
        gravity = GetComponent<GravityComponent>();
        camComponent = GetComponent<CameraController>();
        hurtbox = GetComponent<HurtboxComponent>();

        cam = Camera.main;
    }

    void Start() {
        camComponent.CaptureMouse();

        hurtbox.onHit += OnHit;
    }

    void OnDestroy() {
        hurtbox.onHit -= OnHit;
    }

    void OnDrawGizmos() {
        if (cam != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(cam.transform.position, cam.transform.forward * 10f);
        }
    }

    void Update() {
        UpdateMovement();
        UpdateCamera();
        UpdateWeapon();
    }

#endregion

#region Functions

    private void UpdateMovement() {
        if (controller.isGrounded && jumpInput.action.WasPressedThisFrame()) {
            jump.Jump(velocity);
        }

        if (!controller.isGrounded) {
            gravity.ApplyGravity(velocity);
        }

        finalMove = velocity.Velocity;

        finalMove += movement.GetMovementDirection(transform, movementInput.action.ReadValue<Vector2>());

        controller.Move(finalMove * Time.deltaTime);
    }

    private void UpdateWeapon() {
        if (lightAttackInput.action.ReadValue<float>() > 0f) {
            Debug.Log("Light attack");
        } else if (heavyAttackInput.action.ReadValue<float>() > 0f) {
            Debug.Log("Heavy attack");
        }

        if (weaponSwapInput.action.WasPressedThisFrame()) {
            Debug.Log("Swap weapon");
        }
    }

    private void UpdateCamera() {
        camComponent.RotateCamera(transform, cam, lookInput.action.ReadValue<Vector2>());
    }


    private void OnHit() {
        Debug.Log("I got hit :(");
    }
#endregion
}

