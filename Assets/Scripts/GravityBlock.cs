using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBlock : LevelBlock {
    private bool busy = false;

    void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null || busy) {
            return;
        }

        SphereController sphere = other.transform.parent.GetComponent<SphereController>();

        if (sphere == null) {
            return;
        }

        sphere.SphereBody.velocity = new Vector3();
        sphere.SphereBody.angularVelocity = new Vector3();

        Vector3 gravity = GameController.Instance.GravityDirection;
        gravity.Scale(new Vector3(0, -1, 0));
        GameController.Instance.GravityDirection = gravity;

        BoxBlock[] boxes = FindObjectsOfType<BoxBlock>();
        foreach (BoxBlock box in boxes) {
            box.StartFalling();
        }

        StartCoroutine(WaitFor(1));
    }

    IEnumerator WaitFor(float time) {
        busy = true;
        yield return new WaitForSeconds(time);
        busy = false;
    }
}
