using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour {
    [SerializeField] private List<SpellBehaviour> spells = new List<SpellBehaviour>();
    [SerializeField] private float spellSwapCooldown = 0.25f;
    [SerializeField] private float globalAttackCooldown = 0.1f;

    private SpellBehaviour mainhandSpell;
    private SpellBehaviour offhandSpell;

    private float lightCooldown = 0f;
    private float heavyCooldown = 0f;
    private float swapCooldown = 0f;
    private float _globalAttackCooldown = 0f;

    void Start() {
        if (spells != null && spells.Count > 1) {
            mainhandSpell = spells[0];
            offhandSpell = spells[1];

            mainhandSpell.EquipToMainhand(gameObject);
            offhandSpell.EquipToOffhand(gameObject);

            lightCooldown = mainhandSpell.GetLightAttackCooldown();
            heavyCooldown = mainhandSpell.GetHeavyAttackCooldown();
            swapCooldown = spellSwapCooldown;
            _globalAttackCooldown = globalAttackCooldown;
        }
    }

    void Update() {
        UpdateCooldowns();
    }

    public bool CanLightAttack() => lightCooldown >= mainhandSpell.GetLightAttackCooldown() && _globalAttackCooldown >= globalAttackCooldown;
    public bool CanHeavyAttack() => heavyCooldown >= mainhandSpell.GetHeavyAttackCooldown() && _globalAttackCooldown >= globalAttackCooldown;
    public bool CanSwap() => swapCooldown >= spellSwapCooldown;

    private void UpdateCooldowns() {
        lightCooldown = Mathf.Clamp(lightCooldown + Time.deltaTime, 0f, mainhandSpell.GetLightAttackCooldown());
        heavyCooldown = Mathf.Clamp(heavyCooldown + Time.deltaTime, 0f, mainhandSpell.GetHeavyAttackCooldown());
        swapCooldown = Mathf.Clamp(swapCooldown + Time.deltaTime, 0f, spellSwapCooldown);
        _globalAttackCooldown = Mathf.Clamp(_globalAttackCooldown + Time.deltaTime, 0f, globalAttackCooldown);
    }

    public void SwapSpells() {
        Debug.Log("SpellController: Swapping Spells");

        mainhandSpell.UnequipFromMainhand(gameObject);
        offhandSpell.UnequipFromOffhand(gameObject);

        (mainhandSpell, offhandSpell) = (offhandSpell, mainhandSpell);

        mainhandSpell.EquipToMainhand(gameObject);
        offhandSpell.EquipToOffhand(gameObject);

        lightCooldown = mainhandSpell.GetLightAttackCooldown();
        heavyCooldown = mainhandSpell.GetHeavyAttackCooldown();
        swapCooldown = 0f;
        _globalAttackCooldown = globalAttackCooldown;
    }

    public void LightAttack() {
        Debug.Log("SpellController: Light Attack");
        mainhandSpell.OnLightAttack(gameObject);
        lightCooldown = 0f;
        _globalAttackCooldown = 0f;
    }

    public void HeavyAttack() {
        Debug.Log("SpellController: Heavy Attack");
        mainhandSpell.OnHeavyAttack(gameObject);
        heavyCooldown = 0f;
        _globalAttackCooldown = 0f;
    }
}