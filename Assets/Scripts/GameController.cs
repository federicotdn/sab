using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private const float gravityConst = 9.81f;

    public Vector3 GravityDirection {
        set {
            Vector3 newGrav = value.normalized;
            newGrav.Scale(new Vector3(gravityConst, gravityConst, gravityConst));
            Physics.gravity = newGrav;
        }

        get {
            return Physics.gravity.normalized;
        }
    }

    private static GameController instance;
    public static GameController Instance {
        get {
            if (instance == null) {
                GameController controller = FindObjectOfType<GameController>();
                if (controller != null) {
                    instance = controller;
                } else {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<GameController>();
                }
            }

            return instance;
        }
    }

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CheckVictory() {

    }
}
