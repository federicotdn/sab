using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private const float gravityConst = 9.81f;

    public GameObject gameOverPanel;
    public GameObject gameWonPanel;

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

    public void OnLost() {
        gameOverPanel.SetActive(true);
    }

    public void OnRestartPressed() {
        SceneManager.LoadScene("Level01");
    }

    public void OnMenuPressed() {
        SceneManager.LoadScene("Menu");
    }

    public void CheckVictory() {
        GoalBlock[] goals = FindObjectsOfType<GoalBlock>();
        foreach (GoalBlock goal in goals) {
            if (!goal.Activated) {
                return;
            }
        }

        FindObjectOfType<SphereController>().Freeze();
        gameWonPanel.SetActive(true);
    }
}
