using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class SpellController : MonoBehaviour {
#region Variables

    [SerializeField] List<SpellBehaviour> spells = new List<SpellBehaviour>();
    [SerializeField] float spellSwapCooldown = 0.25f;
    [SerializeField] float globalAttackCooldown = 0.1f;
    [SerializeField] TrailController trailController;
    [SerializeField] LayerMask layerMask;

    SpellBehaviour mainhandSpell;
    SpellBehaviour offhandSpell;
    SpellBehaviour nextSpell;
    List<SpellBehaviour> usedSpells = new List<SpellBehaviour>();
    List<OnHitEffect> dynamicHitEffects = new List<OnHitEffect>();

    Vector3 hitPoint;

    float lightCooldown = 0f;
    float heavyCooldown = 0f;
    float swapCooldown = 0f;
    float _globalAttackCooldown = 0f;

    public static event Action<SpellBehaviour, int> CurrentSpellInfo;
    public static event Action<SpellBehaviour, SpellBehaviour, SpellBehaviour> HeldSpells;

#endregion

#region MonoBehaviour Functions

    void Start() {
        if (spells != null && spells.Count > 1) {
            mainhandSpell = spells[0];
            offhandSpell = spells[1];
            nextSpell = GetNextSpell();

            // mainhandSpell.ReplenishAmmo();
            // offhandSpell.ReplenishAmmo();

            foreach (SpellBehaviour spell in spells) {
                spell.ReplenishAmmo();
            }

            EquipSpell(true, mainhandSpell);
            EquipSpell(false, offhandSpell);

            swapCooldown = spellSwapCooldown;
            _globalAttackCooldown = globalAttackCooldown;

            mainhandSpell.BindToUnlocks(gameObject);

            CurrentSpellInfo?.Invoke(mainhandSpell, mainhandSpell.Ammo);
        }
    }

    void Update() {
        UpdateCooldowns();
    }

    void OnDestroy() {
        if (mainhandSpell != null) {
            mainhandSpell.onAmmoDepleted -= OnMainhandAmmoDepleted;
            mainhandSpell.UnbindToUnlocks(gameObject);
        }
    }

    #endregion

    #region Functions

    public bool CanLightAttack() {
        return lightCooldown >= mainhandSpell.LightAttack.GetFirerate()
            && _globalAttackCooldown >= globalAttackCooldown
            && mainhandSpell.LightAttack.Unlocked();
    }

    public bool CanHeavyAttack() {
        return heavyCooldown >= mainhandSpell.HeavyAttack.GetFirerate()
            && _globalAttackCooldown >= globalAttackCooldown
            && mainhandSpell.HeavyAttack.Unlocked();
    }

    public bool CanSwap() => swapCooldown >= spellSwapCooldown;

    private void UpdateCooldowns() {
        lightCooldown = Mathf.Clamp(lightCooldown + Time.deltaTime, 0f, mainhandSpell.LightAttack.GetFirerate());
        heavyCooldown = Mathf.Clamp(heavyCooldown + Time.deltaTime, 0f, mainhandSpell.HeavyAttack.GetFirerate());
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

        if (mainhandSpell.LightAttack.isHitscan) {
            HitscanAttack(firingPoint, mainhandSpell.LightAttack);
        }

        lightCooldown = 0f;
        _globalAttackCooldown = 0f;

        CurrentSpellInfo?.Invoke(mainhandSpell, mainhandSpell.Ammo);
    }

    public void HeavyAttack(Transform firingPoint) {
        // Debug.Log("SpellController: Heavy Attack");
        mainhandSpell.OnHeavyAttack(gameObject);

        if (mainhandSpell.HeavyAttack.isHitscan) {
            HitscanAttack(firingPoint, mainhandSpell.HeavyAttack);
        }

        heavyCooldown = 0f;
        _globalAttackCooldown = 0f;

        CurrentSpellInfo?.Invoke(mainhandSpell, mainhandSpell.Ammo);
    }

    void HitscanAttack(Transform firingPoint, AttackInfo info) {
        for (int i = 0; i < info.shotCount; i++) {
            Vector3 shotDir = GetSpread(firingPoint.forward, info.spread);

            // Debug.DrawLine(firingPoint.position, firingPoint.position + (shotDir * 10f), Color.green, 5f);

            if (Physics.Raycast(firingPoint.position, shotDir, out RaycastHit hit, float.MaxValue)) {
                // Debug.DrawRay(firingPoint.position, hit.transform.position, Color.blue, 5f);
                trailController.StartCoroutine(trailController.StartTrail(
                    firingPoint.position, hit.point, info.trailConfig
                ));

                hitPoint = hit.point;

                OnHitscanHit(hit.point, info);
            } else {
                trailController.StartCoroutine(trailController.StartTrail(
                    firingPoint.position, firingPoint.position + (shotDir * info.trailConfig.MissFadeDistance), info.trailConfig
                ));
            }
        }
    }

    void ProjectileAttack(Transform firingPoint, AttackInfo info) {

    }

    void OnHitscanHit(Vector3 hitPoint, AttackInfo info) {
        Debug.Log("SpellController: OnHitscan Outer Scope");
        Collider[] colliders = Physics.OverlapSphere(hitPoint, 1f);

        foreach (Collider collider in colliders) {
            IHittable hit = collider.GetComponent<IHittable>();

            if (hit == null) continue;

            Debug.Log("SpellController: OnHitscan Inner Loop");
            hit.OnHit(GetHitEffects(info.attackType), gameObject);
        }
    }

    Vector3 GetSpread(Vector3 initalDirection, Vector3 spread) {
        return initalDirection + new Vector3(
            UnityEngine.Random.Range(-spread.x, spread.x),
            UnityEngine.Random.Range(-spread.y, spread.y),
            UnityEngine.Random.Range(-spread.z, spread.z)
        );
    }

    List<OnHitEffect> GetHitEffects(AttackType attackType) {
        List<OnHitEffect> hitEffects = new();

        switch (attackType) {
            case AttackType.Light:
                hitEffects.AddRange(mainhandSpell.LightAttack.hitEffects);
                break;
            case AttackType.Heavy:
                hitEffects.AddRange(mainhandSpell.HeavyAttack.hitEffects);
                break;
            default:
                break;
        }   

        hitEffects.AddRange(dynamicHitEffects);

        return hitEffects;
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

            lightCooldown = mainhandSpell.LightAttack.GetFirerate();
            heavyCooldown = mainhandSpell.HeavyAttack.GetFirerate();

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

    void OnDrawGizmos() {
        if (hitPoint != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hitPoint, 1f);
        }
    }

    #endregion

    // [Serializable]
    // struct AttackInfo {
    //     public AttackInfo(int shotCount, Vector3 spread, TrailConfig trailConfig, bool isLightAttack) {
    //         this.shotCount = shotCount;
    //         this.spread = spread;
    //         this.trailConfig = trailConfig;
    //         this.isLightAttack = isLightAttack;
    //     }

    //     public int shotCount;
    //     public Vector3 spread;
    //     public TrailConfig trailConfig;
    //     public bool isLightAttack;
    // }
}
