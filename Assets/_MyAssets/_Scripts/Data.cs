using UnityEngine;
using System.Collections;

// This is the script that will handle all savings (Playerprefs)
public class Data : Singleton<Data> 
{
    private Data() { }


    public void SetTotalDistance(float distance)
    {
        PlayerPrefs.SetFloat("TotalDistance", distance);
    }

    public float GetTotalDistance()
    {
        float totalDistance = PlayerPrefs.GetFloat("TotalDistance", 0);
        return totalDistance;
    }

    public void SetTotalScore(int playerScore)
    {
        PlayerPrefs.SetInt("TotalScore", playerScore);
    }

    public int GetTotalScore()
    {
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        return totalScore;
    }
}
