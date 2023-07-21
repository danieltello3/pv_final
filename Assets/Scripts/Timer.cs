using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private string timerText;
    public float totalTime = 120f; // Total time in seconds

    private float currentTime;

    void Start()
    {
        currentTime = totalTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            CanvasManager.Instance.UpdateTime("00:00");
            GameManager.Instance.StopGame();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
        CanvasManager.Instance.UpdateTime(timerText);
    }
}