using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpellController : MonoBehaviour {
#region Variables

    [SerializeField] private List<SpellBehaviour> spells = new List<SpellBehaviour>();
    [SerializeField] private float spellSwapCooldown = 0.25f;
    [SerializeField] private float globalAttackCooldown = 0.1f;
    [SerializeField] private TrailController trailController;
    [SerializeField] private LayerMask layerMask;

    private SpellBehaviour mainhandSpell;
    private SpellBehaviour offhandSpell;
    private SpellBehaviour nextSpell;
    private List<SpellBehaviour> usedSpells = new List<SpellBehaviour>();
    private List<OnHitEffect> dynamicHitEffects = new List<OnHitEffect>();

    private float lightCooldown = 0f;
    private float heavyCooldown = 0f;
    private float swapCooldown = 0f;
    private float _globalAttackCooldown = 0f;

    public static event Action<SpellBehaviour, int> CurrentSpellInfo;
    public static event Action<SpellBehaviour, SpellBehaviour, SpellBehaviour> HeldSpells;

#endregion

#region MonoBehaviour Functions

    void Start() {
        if (spells != null && spells.Count > 1) {
            mainhandSpell = spells[0];
            offhandSpell = spells[1];
            nextSpell = GetNextSpell();

            mainhandSpell.ReplenishAmmo();
            offhandSpell.ReplenishAmmo();

            EquipSpell(true, mainhandSpell);
            EquipSpell(false, offhandSpell);

            swapCooldown = spellSwapCooldown;
            _globalAttackCooldown = globalAttackCooldown;

            CurrentSpellInfo?.Invoke(mainhandSpell, mainhandSpell.Ammo);
        }
    }

    void Update() {
        UpdateCooldowns();
    }

    void OnDestroy() {
        if (mainhandSpell != null) {
            mainhandSpell.onAmmoDepleted -= OnMainhandAmmoDepleted;
        }
    }

    #endregion

    #region Functions

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

        SpellBehaviour temp = offhandSpell;

        EquipSpell(false, mainhandSpell);
        EquipSpell(true, temp);

        swapCooldown = 0f;
        _globalAttackCooldown = globalAttackCooldown;
    }

    public void LightAttack(Transform firingPoint) {
        Debug.Log("SpellController: Light Attack");
        mainhandSpell.OnLightAttack(gameObject);

        if (mainhandSpell.LightAttackIsHitscan) {
            for (int i = 0; i < mainhandSpell.LightAttackShotCount; i++) {
                Vector3 dir = firingPoint.forward + new Vector3(
                    UnityEngine.Random.Range(-mainhandSpell.LightAttackShotSpread.x, mainhandSpell.LightAttackShotSpread.x),
                    UnityEngine.Random.Range(-mainhandSpell.LightAttackShotSpread.y, mainhandSpell.LightAttackShotSpread.y),
                    UnityEngine.Random.Range(-mainhandSpell.LightAttackShotSpread.z, mainhandSpell.LightAttackShotSpread.z)
                );

                if (Physics.Raycast(
                    firingPoint.position,
                    dir,
                    out RaycastHit hit,
                    float.MaxValue,
                    layerMask
                )) {
                    trailController.StartCoroutine(trailController.StartTrail(
                        firingPoint.position,
                        hit.transform.position,
                        mainhandSpell.LightTrailConfig,
                        true
                    ));

                    foreach (Collider collider in Physics.OverlapSphere(hit.transform.position, 1f)) {
                        if (collider.TryGetComponent<HurtboxComponent>(out HurtboxComponent hurtbox)) {
                            hurtbox._OnHurtboxHit(mainhandSpell.LightAttackHitEffects, gameObject);
                        }
                    }
                    // GameObject go = new GameObject("Hitbox");
                    // go.transform.position = hit.transform.position;

                    // SphereCollider collider = go.AddComponent<SphereCollider>();
                    // collider.isTrigger = true;
                    // collider.radius = 1f;

                    // HitboxComponent hitbox = go.AddComponent<HitboxComponent>();
                    // foreach (OnHitEffect effect in mainhandSpell.LightAttackHitEffects) {
                    //     hitbox.AddEffect(effect.Execute);
                    // }

                    // hitbox.collisionLayers = layerMask;

                } else {
                    trailController.StartCoroutine(trailController.StartTrail(
                        firingPoint.position,
                        firingPoint.position + (dir * mainhandSpell.LightTrailConfig.MissFadeDistance),
                        mainhandSpell.LightTrailConfig,
                        true
                    ));
                }
            }
        }


        lightCooldown = 0f;
        _globalAttackCooldown = 0f;

        CurrentSpellInfo?.Invoke(mainhandSpell, mainhandSpell.Ammo);
    }

    public void HeavyAttack(Transform firingPoint) {
        // Debug.Log("SpellController: Heavy Attack");
        mainhandSpell.OnHeavyAttack(gameObject);

        if (mainhandSpell.HeavyAttackIsHitscan) {
            for (int i = 0; i < mainhandSpell.HeavyAttackShotCount; i++) {
                Vector3 dir = firingPoint.forward + new Vector3(
                    UnityEngine.Random.Range(-mainhandSpell.HeavyAttackShotSpread.x, mainhandSpell.HeavyAttackShotSpread.x),
                    UnityEngine.Random.Range(-mainhandSpell.HeavyAttackShotSpread.y, mainhandSpell.HeavyAttackShotSpread.y),
                    UnityEngine.Random.Range(-mainhandSpell.HeavyAttackShotSpread.z, mainhandSpell.HeavyAttackShotSpread.z)
                );

                if (Physics.Raycast(
                    firingPoint.position,
                    dir,
                    out RaycastHit hit,
                    float.MaxValue,
                    layerMask
                )) {
                    trailController.StartCoroutine(trailController.StartTrail(
                        firingPoint.position,
                        hit.transform.position,
                        mainhandSpell.HeavyTrailConfig,
                        false
                    ));
                } else {
                    trailController.StartCoroutine(trailController.StartTrail(
                        firingPoint.position,
                        firingPoint.position + (firingPoint.forward * mainhandSpell.HeavyTrailConfig.MissFadeDistance),
                        mainhandSpell.HeavyTrailConfig,
                        false
                    ));
                }
            }
        }

        heavyCooldown = 0f;
        _globalAttackCooldown = 0f;

        CurrentSpellInfo?.Invoke(mainhandSpell, mainhandSpell.Ammo);
    }

    private void OnMainhandAmmoDepleted() {
        Debug.Log("SpellController: Mainhand Ammo Depleted");
        mainhandSpell.ReplenishAmmo();
        usedSpells.Add(mainhandSpell);
        if (usedSpells.Count >= spells.Count - 1) {
            usedSpells.Clear();
        }


        // SwapSpells();
        foreach (SpellBehaviour spell in spells) {
            Debug.Log("Spell: " + spell.DisplayData.Name);
            if (spell != mainhandSpell && spell != offhandSpell && !usedSpells.Contains(spell)) {
                EquipSpell(true, spell);
                break;
            }
        }

        nextSpell = GetNextSpell();

        HeldSpells?.Invoke(mainhandSpell, offhandSpell, nextSpell);
    }

    private void EquipSpell(bool isMainhand, SpellBehaviour spell) {
        if (isMainhand) {
            mainhandSpell.UnequipFromMainhand(this);
            mainhandSpell.onAmmoDepleted -= OnMainhandAmmoDepleted;

            mainhandSpell = spell;

            mainhandSpell.EquipToMainhand(this);
            // mainhandSpell.ReplenishAmmo();

            mainhandSpell.onAmmoDepleted += OnMainhandAmmoDepleted;

            lightCooldown = mainhandSpell.GetLightAttackCooldown();
            heavyCooldown = mainhandSpell.GetHeavyAttackCooldown();

            CurrentSpellInfo?.Invoke(mainhandSpell, mainhandSpell.Ammo);
        } else {
            offhandSpell.UnequipFromOffhand(this);
            offhandSpell = spell;
            offhandSpell.EquipToOffhand(this);
        }

        HeldSpells?.Invoke(mainhandSpell, offhandSpell, nextSpell);
    }

    private SpellBehaviour GetNextSpell() {
        SpellBehaviour next = null;

        if (usedSpells.Count >= spells.Count - 2) {
            next = usedSpells[0];
        } else {
            foreach (SpellBehaviour spell in spells) {
                if (spell != mainhandSpell && spell != offhandSpell && !usedSpells.Contains(spell)) {
                    next = spell;
                    break;
                }
            }
        }

        return next;
    }

    public void AddHitEffect(OnHitEffect hitEffect) => dynamicHitEffects.Add(hitEffect);
    public bool RemoveHitEffect(OnHitEffect hitEffect) => dynamicHitEffects.Remove(hitEffect);

    #endregion
}
