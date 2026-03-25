using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {
    [Header("Weapons")]
    [SerializeField] List<WeaponData> weapons = new(2);

    // Converts serialized SO data into runtime safe-to-manipulate values
    Dictionary<AttackData, AttackInfo> attacksDictionary = new();
    List<OnHitEffect> dynamicHitEffects = new();
    WeaponData currentWeapon;
    WeaponData offhandWeapon;

    class AttackInfo {
        public bool usesAmmo;
        public int ammoCount;
        public float cooldown;
        public float reloadTime;

        public bool isReloading = false;
        public float currentReloadTime = 0f;
        public float currentCooldown = 0f;

        public AttackInfo(bool usesAmmo, int ammoCount, float cooldown, float reloadTime) {
            this.usesAmmo = usesAmmo;
            this.ammoCount = ammoCount;
            this.cooldown = cooldown;
            this.reloadTime = reloadTime;
        }
    }

    void Start() {
        if (weapons == null || weapons.Count < 2) {
            Debug.LogWarning("Weapons list was either null or contained not enough weapons!");
            return;
        }

        foreach (WeaponData weapon in weapons) {
            foreach (AttackData attack in weapon.attacks.Values) {
                AddAttackToDictionary(attack);
            }
        }

        currentWeapon = weapons[0];
        offhandWeapon = weapons[1];
    }

    void Update() {
        UpdateCooldowns(weapons);
    }

    // TODO: Implement light attack, heavy attack, and ultimate attack
    // TODO: In addition, make sure to add HitscanAttacks, ProjectileAttacks, and Ultimate / Buff / Ability Attacks
    // TODO: Don't forget the milinote, breaks down the whole step by step

    void UpdateCooldowns(List<WeaponData> _weapons) {
        foreach (WeaponData weapon in _weapons) {
            foreach (AttackData attack in weapon.attacks.Values) {
                AttackInfo info = attacksDictionary[attack];
                info.currentCooldown = Mathf.Clamp(info.currentCooldown + Time.deltaTime, 0f, info.cooldown);
                if (info.isReloading) {
                    info.currentReloadTime = Mathf.Clamp(info.currentReloadTime + Time.deltaTime, 0f, info.reloadTime);
                }
            }
        }
    }

    void AddAttackToDictionary(AttackData data) {
        attacksDictionary[data] = new AttackInfo(data.usesAmmo, data.ammoCount, data.cooldown, data.reloadTime);
        Debug.Log($"Added {data.name} to the attacks dictionary!");
    }

    List<OnHitEffect> GetOnHitEffects(AttackData attack) {
        List<OnHitEffect> hitEffects = new(attack.onHitEffects);
        hitEffects.AddRange(dynamicHitEffects);

        return hitEffects;
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
