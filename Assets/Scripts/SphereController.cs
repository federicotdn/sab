using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour {

    private const float MAX_CENTER_DISTANCE = 30.0f;

    private Rigidbody sphereBody;
    private Camera sphereCamera;
    private bool dead = false;

    public Rigidbody SphereBody {
        get { return sphereBody; }
    }

    public Transform cameraAxis;
    public float movementSpeed = 1f;
    public float cameraMoveSpeed = 3f;
    public GameObject sphereObject;

    private bool movementEnabled = true;

	// Use this for initialization
	void Start () {
        sphereBody = GetComponentInChildren<Rigidbody>();
        sphereCamera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (dead) {
            return;
        }

        float hor = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        if (vert != 0 || hor != 0) {
            if (Input.GetKey(KeyCode.LeftControl)) {
                cameraAxis.Rotate(new Vector3(0, hor * cameraMoveSpeed, 0));
            } else if (movementEnabled) {
                float moveHorizontal = hor * movementSpeed;
                float moveVertical = vert * movementSpeed;
                Vector3 movement = sphereCamera.transform.TransformDirection(new Vector3(moveHorizontal, 0.0f, moveVertical));
                movement.y = 0;
                sphereBody.AddForce(movement);
            }
        }

        cameraAxis.position = sphereBody.transform.position;

        if (sphereBody.transform.position.magnitude > MAX_CENTER_DISTANCE) {
            Freeze();
            dead = true;
            GameController.Instance.OnLost();
        }
	}

    public void Freeze() {
        sphereBody.isKinematic = true;
    }

    public void DisableMovementFor(float seconds) {
        StartCoroutine(HideFor(seconds));
    }

    IEnumerator HideFor(float time) {
        movementEnabled = false;
        sphereObject.SetActive(false);
        yield return new WaitForSeconds(time);
        sphereObject.SetActive(true);
        movementEnabled = true;
    }
}
