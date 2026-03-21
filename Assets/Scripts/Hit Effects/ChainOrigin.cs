using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ChainOrigin : MonoBehaviour {
    private int chainCount = 0;
    private float radius = 0f;
    private float timeBeforeChain = 0f;
    private Vector3 origin;

    private float timeBeforeDestroy = 5f;

    public void Initialize(int chainCount, float radius, float timeBeforeChain, Vector3 origin) {
        this.chainCount = chainCount;
        this.radius = radius;
        this.timeBeforeChain = timeBeforeChain;
        this.origin = origin;

        StartCoroutine(ChainTimer());
    }

    private IEnumerator ChainTimer() {
        float t = 0f;

        while (t < timeBeforeChain) {
            t += Time.deltaTime;
            yield return null;
        }

        if (chainCount > 0) {
            foreach (Collider collider in Physics.OverlapSphere(origin, radius)) {
                if (collider.GetComponent<ChainOrigin>() == null) {
                    Debug.DrawLine(origin, collider.transform.position, Color.green, 10f);
                    ChainOrigin newChain = collider.AddComponent<ChainOrigin>();
                    newChain.Initialize(
                        chainCount - 1,
                        radius,
                        timeBeforeChain,
                        collider.transform.position
                    );
                }
            }
        }

        chainCount--;

        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer() {
        yield return new WaitForSeconds(timeBeforeDestroy);

        Destroy(this);
    }

    void OnDrawGizmos() {
        if (origin != Vector3.zero) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}