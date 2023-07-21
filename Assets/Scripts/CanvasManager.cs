using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { private set; get; }

    public TextMeshProUGUI Bullets;
    public TextMeshProUGUI Points;
    public TextMeshProUGUI Targets;
    public TextMeshProUGUI Time;

    public TextMeshProUGUI FinalPoints;

    private int Score = 0;
    private int TargetCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateTargets(TargetCount);
        UpdatePoints(Score);
    }

    public void UpdateBullets(string bullets)
    {
        Bullets.text = bullets;
    }

    public void UpdatePoints(int points)
    {
        Score += points;
        Points.text = Score.ToString();
    }

    public void UpdateTargets(int targetCount)
    {
        TargetCount += targetCount;
        Targets.text = TargetCount.ToString();
    }

    public void UpdateTime(string time)
    {
        Time.SetText(time);
    }

    public void UpdateFinalPoints()
    {
        FinalPoints.text = Score.ToString();
    }

}
