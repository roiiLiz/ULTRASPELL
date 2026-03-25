using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {
    [Header("Weapons")]
    [SerializeField] List<WeaponData> weapons = new(2);

    [Space(5)]

    [Header("Settings")]
    [SerializeField] float globalAttackCooldown = 0.1f;
    [SerializeField] float swapCooldown = 0.25f;

    [Space(5)]

    [Header("References")]
    [SerializeField] TrailController trailController;

    // Converts serialized SO data into runtime safe-to-manipulate values
    Dictionary<AttackData, AttackInfo> attacksDictionary = new();
    Dictionary<WeaponData, int> weaponAmmoDictionary = new();
    List<OnHitEffect> dynamicHitEffects = new();
    WeaponData currentWeapon;
    WeaponData offhandWeapon;

    float _globalCooldown = 0f;
    float _swapCooldown = 0f;

    class AttackInfo {
        public bool usesAmmo;
        public int ammoCost;
        public float cooldown;
        public WeaponData owner;

        public float currentCooldown = 0f;

        public AttackInfo(bool usesAmmo, int ammoCost, float cooldown, WeaponData owner) {
            this.usesAmmo = usesAmmo;
            this.ammoCost = ammoCost;
            this.cooldown = cooldown;
            this.owner = owner;
        }
    }

    void Start() {
        if (weapons == null || weapons.Count < 2) {
            Debug.LogWarning("Weapons list was either null or contained not enough weapons!");
            return;
        }

        foreach (WeaponData weapon in weapons) {
            weaponAmmoDictionary[weapon] = weapon.ammoCount;

            foreach (AttackData attack in weapon.attacks.Values) {
                AddAttackToDictionary(attack, weapon);
            }
        }

        currentWeapon = weapons[0];
        offhandWeapon = weapons[1];

        foreach (HeldEffect effect in currentWeapon.offhandEffects) {
            // effect.OnEquip();

        }
    }

    void Update() {
        UpdateCooldowns(weapons);
    }

    public void SwapWeapons() {

    }

    public void AttemptAttack(Transform firingPoint, AttackLevel attackLevel) {
        AttackData data = null;

        switch (attackLevel) {
            case AttackLevel.Light:
                data = currentWeapon.lightAttack;
                break;
            case AttackLevel.Heavy:
                data = currentWeapon.heavyAttack;
                break;
            case AttackLevel.Ultimate:
                data = currentWeapon.ultimateAttack;
                break;
        }

        if (CanAttack(data)) {
            Attack(firingPoint, data);
        }
    }

    bool CanAttack(AttackData attack) {
        return attacksDictionary[attack].currentCooldown <= 0f
            && _globalCooldown <= 0f
            && attack.IsUnlocked();
    }
    void Attack(Transform firingPoint, AttackData attack) {
        foreach (OnCastEffect castEffect in attack.onCastEffects) {
            // * If possible, change from GameObject to an interface
            castEffect.Execute(gameObject);
        }

        Debug.Log($"{attack.name} ATK Behaviour: {attack.attackBehaviour.GetType().Name}");

        attack.attackBehaviour.Perform(firingPoint, attack, this);

        AttackInfo info = attacksDictionary[attack];
        info.currentCooldown = info.cooldown;
        // if (info.usesAmmo) {
        //     weaponAmmoDictionary[info.owner] -= info.ammoCost;

        //     if (weaponAmmoDictionary[info.owner] <= 0) {

        //     }
        // }

        _globalCooldown = globalAttackCooldown;
    }

    public void CreateTrail(Vector3 start, Vector3 end, TrailConfig config) => trailController.StartCoroutine(trailController.StartTrail(start, end, config));

    // TODO: Implement light attack, heavy attack, and ultimate attack
    // TODO: In addition, make sure to add HitscanAttacks, ProjectileAttacks, and Ultimate / Buff / Ability Attacks
    // TODO: Don't forget the milinote, breaks down the whole step by step

    // TODO: Don't forget: When possible, code to an interface! Ex. ApplyOnHitEffects(IDamageable target, IDamageable owner) -> cast the interface using the 'as' keyword
    // TODO: ex. (HealthComponent health = target as HealthComponent)!

    void UpdateCooldowns(List<WeaponData> _weapons) {
        foreach (WeaponData weapon in _weapons) {
            foreach (AttackData attack in weapon.attacks.Values) {
                AttackInfo info = attacksDictionary[attack];
                info.currentCooldown = Mathf.Clamp(info.currentCooldown - Time.deltaTime, 0f, info.cooldown);
                // if (info.isReloading) {
                //     info.currentReloadTime = Mathf.Clamp(info.currentReloadTime - Time.deltaTime, 0f, info.reloadTime);
                // }
            }
        }

        _globalCooldown = Mathf.Clamp(_globalCooldown - Time.deltaTime, 0f, globalAttackCooldown);
    }

    void AddAttackToDictionary(AttackData data, WeaponData owner) {
        attacksDictionary[data] = new AttackInfo(data.usesAmmo, data.ammoCost, data.cooldown, owner);
        Debug.Log($"Added {data.name} to the attacks dictionary!");
    }

    List<OnHitEffect> GetOnHitEffects(AttackData attack) {
        List<OnHitEffect> hitEffects = new(attack.onHitEffects);
        hitEffects.AddRange(dynamicHitEffects);

        return hitEffects;
    }

    public void OnHit(AttackData data, IDamageable target) {
        foreach (OnHitEffect effect in GetOnHitEffects(data)) {
            effect.Execute(target);
        }
    }

    public void AddDynamicHitEffect(OnHitEffect hitEffect) {
        dynamicHitEffects.Add(hitEffect);
    }

    public void RemoveDynamicHitEffect(OnHitEffect hitEffect) {
        dynamicHitEffects.Remove(hitEffect);
    }
}

public class WeaponSpread {
    public static Vector3 GetSpread(Vector3 direction, Vector3 spread) {
        return direction + new Vector3(
            UnityEngine.Random.Range(-spread.x, spread.x),
            UnityEngine.Random.Range(-spread.y, spread.y),
            UnityEngine.Random.Range(-spread.z, spread.z)
        );
    }
}

public struct DisplayData {
    public string Name;
    [TextArea(4, 10)]
    public string Description;
}

public enum AttackType {
    Hitscan,
    Projectile,
    Ability
}

public enum AttackLevel {
    Light,
    Heavy,
    Ultimate
}
