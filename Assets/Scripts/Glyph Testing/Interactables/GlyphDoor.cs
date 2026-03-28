using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlyphDoor : MonoBehaviour, IGlyphInteractable {
    [SerializeField] Glyph unlockGlyph;
    [SerializeField] Vector3 openedPosition;
    [SerializeField] float timeToOpen = 0.5f;
    Vector3 closedPosition;
    bool closed = true;

    NewPlayer player;

    void Start() {
        closedPosition = transform.position;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<NewPlayer>();

        player.OnGlyphMatched += Interact;
    }

    void OnDestroy() => player.OnGlyphMatched -= Interact;

    public Glyph GetGlyph => unlockGlyph;

    public void Interact(GlyphData glyphData) {
        if (glyphData.glyph == unlockGlyph) {
            // unlock logic
            StartCoroutine(ToggleDoor(closed));
        }
    }

    IEnumerator ToggleDoor(bool openDoor) {
        float t = 0f;
        Vector3 a = openDoor ? closedPosition : openedPosition;
        Vector3 b = openDoor ? openedPosition : closedPosition;

        while (t < timeToOpen) {
            t += Time.deltaTime;

            transform.position = Vector3.Lerp(a, b, t * (1f / timeToOpen));

            yield return null;
        }

        closed = !closed;
    }
}