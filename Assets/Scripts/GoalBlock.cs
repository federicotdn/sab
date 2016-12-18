using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : LevelBlock {

    private bool activated = false;
    public bool Activated {
        get { return activated; }
    }

    void OnTriggerEnter(Collider other) {
        if (activated) {
            return;
        }

        BoxBlock box = other.GetComponent<BoxBlock>();

        if (box == null) {
            return;
        }

        activated = true;
        box.Freeze();
        GameController.Instance.CheckVictory();
    }
}
