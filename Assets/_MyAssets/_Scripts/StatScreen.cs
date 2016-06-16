using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatScreen : MonoBehaviour {

    public Text bestDistance, totalDistance, bestScore, totalScore;
    private float bDistance, tDistance;
    private int bScore, tScore;

    private void GetData()
    {
        bDistance = Data.Instance.GetBestDistance();
        tDistance = Data.Instance.GetTotalDistance();
        bScore = Data.Instance.GetBestScore();
        tScore = Data.Instance.GetTotalScore();
    }

    private void SetData()
    {
        bestDistance.text = string.Format("{0:0000000}" + "m", bDistance);
        totalDistance.text = string.Format("{0:0000000}" + "m", tDistance);
        bestScore.text = string.Format("{0:00000000}", bScore);
        totalScore.text = string.Format("{0:00000000}", tScore);
    }

    public void UpdateData()
    {
        GetData();
        SetData();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.B))
        {
            Debug.LogWarning("LOADING GAME SCENE WILL RESULT IN ERRORS, STOP GAME AND START AGAIN");
            Data.Instance.ResetStats();
            UpdateData();
        }
    }
}
