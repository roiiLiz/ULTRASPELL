using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

#region Variables

    public static event Action<int, int> Damaged;
    public static event Action Died;


    [Header("Input Actions")]
    [SerializeField] InputActionReference movementInput;
    [SerializeField] InputActionReference jumpInput;
    [SerializeField] InputActionReference lookInput;
    [SerializeField] InputActionReference lightAttackInput;
    [SerializeField] InputActionReference heavyAttackInput;
    [SerializeField] InputActionReference weaponSwapInput;

    [Space(10)]

    [Header("Debug Settings")]
    [SerializeField] InputActionReference slowTimeInput;
    [SerializeField, Range(0f, 1f)] float slowTimeScale = 0.25f;

    CharacterController controller;
    VelocityComponent velocity;
    MovementComponent movement;
    JumpComponent jump;
    GravityComponent gravity;
    CameraController camComponent;
    HurtboxComponent hurtbox;
    SpellController spellController;
    AttackController attackController;
    HealthComponent health;

    Camera cam;

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
        health = GetComponent<HealthComponent>();
        attackController = GetComponent<AttackController>();

        cam = Camera.main;
    }

    void Start() {
        camComponent.CaptureMouse();

        // hurtbox.onHit += OnHit;
        health.OnDamaged += OnDamaged;
        health.OnDied += OnDied;
    }

    void OnDestroy() {
        // hurtbox.onHit -= OnHit;
        health.OnDamaged -= OnDamaged;
        health.OnDied -= OnDied;
    }

    void OnDied() => Died?.Invoke();

    void OnDamaged(int currentHealth, int damageValue) => Damaged?.Invoke(currentHealth, damageValue);

    void OnDrawGizmos() {
        if (cam != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(cam.transform.position, cam.transform.position + (cam.transform.forward * 10f));
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
        // if (lightAttackInput.action.ReadValue<float>() > 0f && spellController.CanLightAttack()) {
        //     // Debug.Log("Light attack");
        //     spellController.LightAttack(cam.transform);
        // } else if (heavyAttackInput.action.ReadValue<float>() > 0f && spellController.CanHeavyAttack()) {
        //     // Debug.Log("Heavy attack");
        //     spellController.HeavyAttack(cam.transform);
        // }

        // if (weaponSwapInput.action.WasPressedThisFrame() && spellController.CanSwap()) {
        //     // Debug.Log("Swap weapon");
        //     spellController.SwapSpells();
        // }

        if (lightAttackInput.action.ReadValue<float>() > 0f) {
            attackController.AttemptAttack(cam.transform, AttackLevel.Light);
        } else if (heavyAttackInput.action.ReadValue<float>() > 0f) {
            attackController.AttemptAttack(cam.transform, AttackLevel.Heavy);
        }
    }

    private void UpdateCamera() {
        camComponent.RotateCamera(transform, cam, lookInput.action.ReadValue<Vector2>());
    }

#endregion
}

