using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour {
    public void StartGame() {
        SceneManager.LoadScene("Game");
    }

    public void EditGestures() {
        SceneManager.LoadScene("Editor");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
