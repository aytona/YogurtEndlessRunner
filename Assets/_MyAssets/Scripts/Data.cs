﻿using UnityEngine;
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

    public void SetBestDistance(float distance)
    {
        float lastDistance = PlayerPrefs.GetFloat("BestDistance", 0);
        if (distance > lastDistance)
        {
            PlayerPrefs.SetFloat("BestDistance", distance);
        }
        else
        {
            PlayerPrefs.SetFloat("BestDistance", lastDistance);
        }
    }

    public float GetBestDistance()
    {
        float bestDistance = PlayerPrefs.GetFloat("BestDistance", 0);
        return bestDistance;
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

    public void SetBestScore(int playerScore)
    {
        int lastScore = PlayerPrefs.GetInt("BestScore", 0);
        if (playerScore > lastScore)
        {
            PlayerPrefs.SetInt("BestScore", playerScore);
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", lastScore);
        }
    }

    public int GetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        return bestScore;
    }

    public void ResetStats()
    {
        PlayerPrefs.SetFloat("TotalDistance", 0);
        PlayerPrefs.SetFloat("BestDistance", 0);
        PlayerPrefs.SetInt("TotalScore", 0);
        PlayerPrefs.SetInt("BestScore", 0);
    }
}
