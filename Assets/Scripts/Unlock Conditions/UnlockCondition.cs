using UnityEngine;

public interface IUnlockCondition {
    public bool Evaluate();
    public void BindEvaluation(GameObject evaluatedObject);
    public void UnbindEvaluation(GameObject evaluatedObject);
}

public abstract class UnlockCondition : ScriptableObject, IUnlockCondition {
    public abstract bool Evaluate();
    public abstract void BindEvaluation(GameObject evaluatedObject);
    public abstract void UnbindEvaluation(GameObject evaluatedObject);
}
