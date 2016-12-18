using System.Collections;
using UnityEngine;

public class TeleportBlock : LevelBlock {

    [Tooltip("Set only if this teleport block is an entrance.")]
    public TeleportBlock exitBlock = null;
    public GameObject teleportEffect;

    private GameObject lastEffect = null;

    private bool busy = false;

    void OnTeleportedTo() {
        if (lastEffect != null) {
            Destroy(lastEffect);
            lastEffect = null;
        }

        lastEffect = Instantiate(teleportEffect);
        lastEffect.transform.position = transform.position + new Vector3(0, 1, 0);
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null || busy || exitBlock == null) {
            return;
        }

        SphereController sphere = other.transform.parent.GetComponent<SphereController>();
        if (sphere == null) {
            return;
        }

        if (sphere.SphereBody.transform.position.y < transform.position.y + 0.9f) {
            // Sphere must be above block to activate it
            return;
        }

        sphere.SphereBody.velocity = new Vector3();
        sphere.SphereBody.angularVelocity = new Vector3();

        sphere.SphereBody.transform.position = exitBlock.transform.position + new Vector3(0, 1, 0);
        sphere.DisableMovementFor(3);
        exitBlock.OnTeleportedTo();

        Debug.Log("teleport");
        StartCoroutine(WaitFor(2));
    }

    IEnumerator WaitFor(float time) {
        busy = true;
        yield return new WaitForSeconds(time);
        busy = false;
    }
}
