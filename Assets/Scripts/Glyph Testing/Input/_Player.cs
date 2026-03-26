using System;
using System.Drawing;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class _Player : MonoBehaviour {
    public event Action<bool> OnRightClick;
    
    [Header("Input")]
    [SerializeField] InputActionReference leftClick;
    [SerializeField] InputActionReference rightClick;
    [SerializeField] InputActionReference mousePosition;

    [Space(5)]

    [Header("Settings")]
    [SerializeField] Transform weaponMuzzle;

    GlyphController glyphController; 
    WeaponController weaponController;
    Camera cam;

    void Awake() {
        glyphController = GetComponent<GlyphController>();
        weaponController = GetComponent<WeaponController>();
        cam = Camera.main;
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
            // glyphController.ToggleGlyphDrawing();
        }
    }
}
