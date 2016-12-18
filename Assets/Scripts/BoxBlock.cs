using System.Collections;
using UnityEngine;

public class BoxBlock : LevelBlock {

    public Collider solidCollider;
    public float minPushSpeed = 0.1f;

    private Rigidbody body;
    private bool busy = false;
    private bool falling = false;

    void Start() {
        body = GetComponent<Rigidbody>();
    }

    void Update() {
        if (falling) {
            LevelBlock block = GetBlockAt(GameController.Instance.GravityDirection);
            if (block != null) {
                solidCollider.enabled = true;
                falling = false;
            }
        }
    }

    public void Freeze() {
        busy = true;
        body.isKinematic = true;
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.parent == null || busy || falling) {
            return;
        }

        SphereController sphere = other.transform.parent.GetComponent<SphereController>();

        if (sphere == null || sphere.SphereBody.velocity.magnitude < minPushSpeed) {
            return;
        }

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

        LevelBlock next = GetBlockAt(pushDirection);

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

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        LevelBlock block = GetBlockAt(GameController.Instance.GravityDirection);
        if (block == null) {
            // Nothing underneath us, disable solid collider so we can fall
            StartFalling();
        }

        if (!body.isKinematic) {
            busy = false;
        }
    }

    public void StartFalling() {
        falling = true;
        solidCollider.enabled = false;
    }

    IEnumerator WaitFor(float time) {
        busy = true;
        yield return new WaitForSeconds(time);
        busy = false;
    }
}
