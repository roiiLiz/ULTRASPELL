using UnityEngine;

public abstract class OnHitEffect : ScriptableObject {
    public Targeting Targeting;
    public abstract void Execute(Collider target, GameObject owner);
}

public enum Targeting {
    Target,
    Owner
}
