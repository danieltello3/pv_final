using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public void TakeHit()
    {
        Destroy(gameObject);
        CanvasManager.Instance.UpdateTargets(1);
    }

    public void ManagePoints(float distance)
    {
        int num = PointsConverter(distance);
        CanvasManager.Instance.UpdatePoints(num);
    }

    public int PointsConverter(float distance)
    {
        int points;

        if (distance <= 10)
            points = 10;
        else if (distance <= 20)
            points = 20;
        else if (distance <= 30)
            points = 30;
        else if (distance <= 40)
            points = 40;
        else if (distance <= 50)
            points = 50;
        else if (distance <= 60)
            points = 60;
        else if (distance <= 70)
            points = 70;
        else if (distance <= 80)
            points = 80;
        else if (distance <= 90)
            points = 90;
        else if (distance <= 100)
            points = 100;
        else if (distance <= 110)
            points = 110;
        else
            points = 120;
        
        return points;
    }
}
