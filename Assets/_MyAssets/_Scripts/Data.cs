using UnityEngine;
using System.Collections;

// This is the script that will handle all savings (Playerprefs)
public class Data : Singleton<Data> 
{
    private Data() { }


    public void SetTotalDistance(float distance)
    {
        float totalDistance = PlayerPrefs.GetFloat("TotalDistance", 0);
        totalDistance += distance;
        PlayerPrefs.SetFloat("TotalDistance", totalDistance);
    }

    public float GetTotalDistance()
    {
        float totalDistance = PlayerPrefs.GetFloat("TotalDistance", 0);
        return totalDistance;
    }

    public void SetTotalScore(int playerScore)
    {
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        totalScore += playerScore;
        PlayerPrefs.SetInt("TotalScore", totalScore);
    }

    public int GetTotalScore()
    {
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        return totalScore;
    }
}
