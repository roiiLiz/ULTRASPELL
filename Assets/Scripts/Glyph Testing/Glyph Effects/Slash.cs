using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slash : MonoBehaviour {
    MovementComponent movement;

    void Start() {
        movement = GetComponent<MovementComponent>();       
    }

    void Update() {
        movement.MoveTransformInDirection(transform, transform.forward);
    }
}
