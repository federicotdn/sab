using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBlock : LevelBlock {

    public Collider solidCollider;
    public Rigidbody body;

    private bool busy = false;
    private bool falling = false;

    void Update() {
        if (falling) {
            LevelBlock block = GetBlockAt(new Vector3(0, -1, 0));
            if (block != null) {
                solidCollider.enabled = true;
                falling = false;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null || busy || falling) {
            return;
        }

        SphereController sphere = other.transform.parent.GetComponent<SphereController>();
        if (sphere == null) {
            return;
        }

        Debug.Log("Bumperino");

        sphere.SphereBody.velocity = new Vector3();
        sphere.SphereBody.angularVelocity = new Vector3();

        Vector3 pushDirection = transform.position - sphere.SphereBody.transform.position;
        if (Mathf.Abs(pushDirection.y) > 0.1f) {
            // Can only push box if on the same y coordinate
            return;
        }

        pushDirection.y = 0;

        if (Mathf.Abs(pushDirection.x) > Mathf.Abs(pushDirection.z)) {
            pushDirection.x = Mathf.Sign(pushDirection.x);
            pushDirection.z = 0;
        } else {
            pushDirection.z = Mathf.Sign(pushDirection.z);
            pushDirection.x = 0;
        }

        Debug.Log(pushDirection);

        LevelBlock next = GetBlockAt(pushDirection);
        Debug.Log(next != null ? "block present" : "nothing");

        if (next == null) {
            // Clear to move in push direction
            Vector3 moveTo = transform.position + pushDirection;
            StartCoroutine(MoveTo(transform.position, moveTo, 1));
        } else {
            // Wait some time for next collision
            StartCoroutine(WaitFor(1));
        }
    }

    private LevelBlock GetBlockAt(Vector3 direction) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out hit, 1)) {
            GameObject go = hit.collider.gameObject;
            return go.GetComponent<LevelBlock>();
        }

        return null;
    }

    private IEnumerator MoveTo(Vector3 start, Vector3 finish, float time) {
        float elapsedTime = 0;
        busy = true;

        while (elapsedTime < time) {
            float x = Mathf.Lerp(start.x, finish.x, (elapsedTime / time));
            float z = Mathf.Lerp(start.z, finish.z, (elapsedTime / time));
            transform.position = new Vector3(x, transform.position.y, z);
            Debug.Log(transform.position);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        LevelBlock block = GetBlockAt(new Vector3(0, -1, 0));
        if (block == null) {
            // Nothing underneath us, disable solid collider so we can fall
            falling = true;
            solidCollider.enabled = false;
        }

        busy = false;
    }

    IEnumerator WaitFor(float time) {
        busy = true;
        yield return new WaitForSeconds(time);
        busy = false;
    }
}
