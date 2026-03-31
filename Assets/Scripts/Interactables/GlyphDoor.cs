using System.Collections;
using UnityEngine;

public class GlyphDoor : MonoBehaviour, IGlyphInteractable {
    [SerializeField] Glyph unlockGlyph;
    [SerializeField] GameObject door;
    [SerializeField] Vector3 openedPosition;
    [SerializeField] float timeToOpen = 0.5f;
    Vector3 closedPosition;
    bool closed = true;

    void Start() {
        closedPosition = door.transform.position;
    }

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

            door.transform.position = Vector3.Lerp(a, b, t / timeToOpen);

            yield return null;
        }

        Debug.Log("Hello!");
        closed = !closed;
    }
}