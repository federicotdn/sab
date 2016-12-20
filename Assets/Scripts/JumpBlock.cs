using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlock : LevelBlock {

    private bool busy;
    public float boostStrength = 10.0f;

    void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null || busy) {
            return;
        }

        SphereController sphere = other.transform.parent.GetComponent<SphereController>();
        if (sphere == null) {
            return;
        }

        sphere.SphereBody.AddForce(new Vector3(0, boostStrength, 0));
        StartCoroutine(WaitFor(2));
    }

    IEnumerator WaitFor(float time) {
        busy = true;
        yield return new WaitForSeconds(time);
        busy = false;
    }
}
