using UnityEngine;

public abstract class OnHitEffect : ScriptableObject {
    public abstract void Execute(Collider target, GameObject owner);
}
