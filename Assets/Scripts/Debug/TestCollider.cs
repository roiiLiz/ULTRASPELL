using System.Collections.Generic;
using UnityEngine;

public class TestCollider : MonoBehaviour {

    private HitboxComponent hitbox;

    public List<OnHitEffect> OnHitEffects = new();
    public OnHitEffect removeThing;

    void Awake() {
        hitbox = GetComponent<HitboxComponent>();
        // hitbox.AddEffect(OnHit);

        foreach (OnHitEffect effect in OnHitEffects) {
            hitbox.AddEffect(effect.Execute);
        }

        hitbox.RemoveEffect(removeThing.Execute);
    }
}
