using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour {

    private Rigidbody sphereBody;
    private Camera sphereCamera;

    public Rigidbody SphereBody {
        get { return sphereBody; }
    }

    public Transform cameraAxis;
    public float movementSpeed = 1f;
    public float cameraMoveSpeed = 3f;

	// Use this for initialization
	void Start () {
        sphereBody = GetComponentInChildren<Rigidbody>();
        sphereCamera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        float hor = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        if (vert != 0 || hor != 0) {
            if (Input.GetKey(KeyCode.LeftControl)) {
                cameraAxis.Rotate(new Vector3(0, hor * cameraMoveSpeed, 0));
            } else {
                float moveHorizontal = hor * movementSpeed;
                float moveVertical = vert * movementSpeed;
                Vector3 movement = sphereCamera.transform.TransformDirection(new Vector3(moveHorizontal, 0.0f, moveVertical));
                movement.y = 0;
                sphereBody.AddForce(movement);
            }
        }

        cameraAxis.position = sphereBody.transform.position;
	}
}
