using UnityEngine;

public class AbilityAttack : IAttack {
    public void Perform(Transform firingPoint, AttackData data, AttackController controller) {
        Debug.Log("Performing ability attack!");

        return;
    }
}
