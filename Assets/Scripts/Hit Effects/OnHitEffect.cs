using UnityEngine;

public abstract class OnHitEffect : ScriptableObject {
    public Targeting Targeting;
    public abstract void Execute(GameObject target);
}

public enum Targeting {
    Target,
    Owner
}
