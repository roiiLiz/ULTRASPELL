using UnityEngine;

public interface IWeapon {
    public WeaponConfig GetConfig { get; }
    public void Fire(Transform firingPoint, Transform tracerOrigin);
}
