using UnityEngine;

public interface IAttack {
    void Perform(Transform firingPoint, AttackData data, AttackController controller);
}
