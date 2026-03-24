using UnityEngine;

[CreateAssetMenu(fileName = "New No Hit Condition", menuName = "Unlock Conditions / No Hit Condition")]
public class NoHitCondition : UnlockCondition {
    [SerializeField] float noHitDuration = 10f;
    float noHitTime = 0f;

    private void SetTime(float time) => noHitTime = time;
    public override void BindEvaluation(GameObject evaluatedObject) {
        if (evaluatedObject.TryGetComponent<HealthComponent>(out var health)) {
            health.NoHitDuration += SetTime;
        }
    }

    public override void UnbindEvaluation(GameObject evaluatedObject) {
        if (evaluatedObject.TryGetComponent<HealthComponent>(out var health)) {
            health.NoHitDuration -= SetTime;
        }
    }

    public override bool Evaluate() {
        Debug.Log($"-- Evaluating no hit conditional --\nCurrent no hit time: {noHitTime}\nConditional no hit duration: {noHitDuration}\nEvaluation: {noHitTime >= noHitDuration}");
        return noHitTime >= noHitDuration;
    }
}
