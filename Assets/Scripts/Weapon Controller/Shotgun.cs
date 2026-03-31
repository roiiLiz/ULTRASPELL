using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon {
    [SerializeField] WeaponConfig config;
    public WeaponConfig GetConfig => config;

    public void Fire(Transform firingPoint, Transform tracerOrigin) {
        return;
        // shotgun firing logic
    }
}
