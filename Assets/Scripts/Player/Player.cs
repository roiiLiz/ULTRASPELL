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

    [Space(10)]

    [Header("Debug Settings")]
    [SerializeField] private InputActionReference slowTimeInput;
    [SerializeField, Range(0f, 1f)] private float slowTimeScale = 0.25f;

    private CharacterController controller;
    private VelocityComponent velocity;
    private MovementComponent movement;
    private JumpComponent jump;
    private GravityComponent gravity;
    private CameraController camComponent;
    private HurtboxComponent hurtbox;
    private SpellController spellController;

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
        spellController = GetComponent<SpellController>();

        cam = Camera.main;
    }

    void Start() {
        camComponent.CaptureMouse();

        // hurtbox.onHit += OnHit;
    }

    void OnDestroy() {
        // hurtbox.onHit -= OnHit;
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

        if (slowTimeInput.action.WasPressedThisFrame()) {
            Time.timeScale = Time.timeScale == slowTimeScale ? 1f : slowTimeScale;
        }
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

        Vector3 moveDir = new Vector3(movementInput.action.ReadValue<Vector2>().x, 0f, movementInput.action.ReadValue<Vector2>().y);

        finalMove += movement.GetMovementDirection(transform, moveDir);

        controller.Move(finalMove * Time.deltaTime);
    }

    private void UpdateWeapon() {
        if (lightAttackInput.action.ReadValue<float>() > 0f && spellController.CanLightAttack()) {
            // Debug.Log("Light attack");
            spellController.LightAttack(cam.transform);
        } else if (heavyAttackInput.action.ReadValue<float>() > 0f && spellController.CanHeavyAttack()) {
            // Debug.Log("Heavy attack");
            spellController.HeavyAttack(cam.transform);
        }

        if (weaponSwapInput.action.WasPressedThisFrame() && spellController.CanSwap()) {
            // Debug.Log("Swap weapon");
            spellController.SwapSpells();
        }
    }

    private void UpdateCamera() {
        camComponent.RotateCamera(transform, cam, lookInput.action.ReadValue<Vector2>());
    }

#endregion
}

