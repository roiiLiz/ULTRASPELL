using UnityEngine;

public class ProjectileAttack : IAttack {
    public void Perform(Transform firingPoint, AttackData data, AttackController controller) {
        Debug.Log("Performing projectile attack!");
        if (data.projectilePrefab == null) {
            Debug.LogWarning($"{data.name} does not have a projectile prefab!");
            return;
        }

        for (int i = 0; i < data.shotCount; i++) {
            Vector3 dir = WeaponSpread.GetSpread(firingPoint.forward, data.spread);


            Projectile projectile = GameObject.Instantiate(data.projectilePrefab, firingPoint.position, Quaternion.identity).GetComponent<Projectile>();

            projectile.direction = dir;
            projectile.Initialize(data, controller);
        }        
    }
}
