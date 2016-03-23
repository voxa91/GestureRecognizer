using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    static public GameController Instance;

    public InputField gestureNameField;
    public LineRenderer inputGestureLineRenderer;
    public LineRenderer currentGestureLineRenderer;
    public TrailRenderer fingerTrailRender;

    public Text timerText;
    public Text scoreText;
    public Text percentText;

    public Text gameOverScoreText;
    public CanvasGroup gameOverWindow;

    public bool editorMode;

    private List<Vector2> points;

    private Gestures gestures;
    public string gesturesFileName;
    private int currentGesture;

    private const float maxTimerValue = 15f;
    private const float timerDecreaseValue = 1f;
    private float currentTime;
    private int score;

    private bool gameOver;

    public bool IsGameOver {
        get { return gameOver; }
    }

    public bool IsEditorMode {
        get { return editorMode; }
    }

    void Awake() {
        Instance = this;
    }

	void Start () {
        points = new List<Vector2>();

        gestures = new Gestures(gesturesFileName);
        currentGesture = 0;
        ShowCurrentGesture();

        gameOver = false;
        if (!IsEditorMode) {
            score = 0;
            UpdateScoreText(scoreText);
            ResetTimer();
            gameOverWindow.gameObject.SetActive(false);
        }
	}

    public Gesture GetCurrentGesture() {
        return gestures.GesturesList[currentGesture];
    }

    public void ResetTimer() {
        currentTime = Mathf.Clamp (maxTimerValue - score * timerDecreaseValue, 2, maxTimerValue);
        UpdateTimerText();
    }

    public void UpdateTimerText() {
        timerText.text = currentTime.ToString("0.00");
    }

    public void UpdateScoreText(Text text) {
        text.text = "Score: " + score.ToString();
    }

    public void NextGesture() {
        currentGesture++;
        if (currentGesture >= gestures.GesturesList.Count) {
            currentGesture = 0;
        }
        ShowCurrentGesture();
    }

    public void PrevGesture() {
        currentGesture--;
        if (currentGesture < 0) {
            currentGesture = gestures.GesturesList.Count - 1;
        }
        ShowCurrentGesture();
    }

    private void ShowCurrentGesture() {
        Vector2 shift = Vector2.right * GetScreen.width * 0.25f;
        Gesture gesture = gestures.GesturesList[currentGesture];
        currentGestureLineRenderer.SetVertexCount(gesture.points.Count);
        for (int i = 0; i < gesture.points.Count; i++) {
            currentGestureLineRenderer.SetPosition(i, gesture.points[i] / 100f + shift);
        }
    }

    public void ClearPoints() {
        points.Clear();
        if (IsEditorMode) {
            inputGestureLineRenderer.SetVertexCount(0);
        }
    }

    public void AddPoint(Vector3 point) {
        points.Add(point);
        if (IsEditorMode) {
            inputGestureLineRenderer.SetVertexCount(points.Count);
            inputGestureLineRenderer.SetPosition(points.Count - 1, Camera.main.ScreenToWorldPoint(point + Vector3.forward * 10));
        } else { 
            fingerTrailRender.transform.position = Camera.main.ScreenToWorldPoint(point + Vector3.forward * 10);
        }
    }

    public void CheckGesture() {
        if (IsEditorMode) return;

        Gesture gesture = new Gesture(points);
        float result = Gesture.CompareGestures(gestures.GesturesList[currentGesture], gesture);
        percentText.text = (int)(result * 100) + "%";
        if (result >= 0.85f) {
            score++;
            UpdateScoreText(scoreText);
            NextGesture();
            ResetTimer();
        }
    }

    public void AddGesture() {
        Gesture newGesture = new Gesture(points);
        gestures.GesturesList.Add(newGesture);
    }

    public void RemoveGesture() {
        gestures.GesturesList.RemoveAt(currentGesture);
        if (currentGesture >= gestures.GesturesList.Count) {
            currentGesture = 0;
        }
        ShowCurrentGesture();
    }

    public void GoToMenu() {
        if (IsEditorMode) {
            gestures.SaveJson();
        }
        SceneManager.LoadScene("Menu");
    }

    public void Restart() {
        SceneManager.LoadScene("Game");
    }

    private IEnumerator ShowGameOver() {
        UpdateScoreText(gameOverScoreText);
        gameOverWindow.alpha = 0;
        gameOverWindow.gameObject.SetActive(true);
        float speed = 2f;
        while (gameOverWindow.alpha < 1) {
            gameOverWindow.alpha += Time.deltaTime * speed;
            yield return null;
        }
    }

    public void Update() {
        if (IsEditorMode) return;

        if (!IsGameOver) {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) {
                currentTime = 0;
                gameOver = true;
                StartCoroutine(ShowGameOver());                
            }
            UpdateTimerText();
        }
    }
}
