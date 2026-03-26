using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon {
    [SerializeField] WeaponConfig config;
    public WeaponConfig GetConfig => config;

    public void Fire(Transform firingPoint, Transform tracerOrigin) {
        return;
        // glock firing logic
    }
}
