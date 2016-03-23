using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenInput : MonoBehaviour {
    private Rect drawArea;

    void Start() {
        drawArea = new Rect(0, 0, Screen.width / 2, Screen.height);
    }

    void Update() {
        if (!GameController.Instance.IsGameOver) {
            if (drawArea.Contains(Input.mousePosition)) {
                if (Input.GetMouseButtonDown(0)) {
                    GameController.Instance.ClearPoints();
                } else if (Input.GetMouseButton(0)) {
                    GameController.Instance.AddPoint(Input.mousePosition);
                } else if (Input.GetMouseButtonUp(0)) {
                    GameController.Instance.CheckGesture();
                }
            }
        }
    }
}
