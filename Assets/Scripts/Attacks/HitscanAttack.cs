using UnityEngine;

public class HitscanAttack : IAttack {
    public void Perform(Transform firingPoint, AttackData data, AttackController controller) {
        Debug.Log("Performing hitscan attack!");

        for (int i = 0; i < data.shotCount; i++) {
            Vector3 dir = WeaponSpread.GetSpread(firingPoint.forward, data.spread);

            if (Physics.Raycast(firingPoint.position, dir, out RaycastHit hit, float.MaxValue)) {
                if (hit.transform.TryGetComponent<IDamageable>(out var damageable)) {
                    controller.OnHit(data, damageable);
                }

                controller.CreateTrail(firingPoint.position, hit.point, data.trailConfig);
            } else {
                controller.CreateTrail(firingPoint.position, firingPoint.position + (dir * data.trailConfig.MissFadeDistance), data.trailConfig);
            }
        }        
    }
}
