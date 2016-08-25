using UnityEngine;
using System.Collections;

public class CharacterSelectAnimation : MonoBehaviour {
    public Animator[] m_Animator;
    private string[] triggerNames = { "Selected", "Wave" };
    public GameObject[] spoon;
    private float randomTime;
    public SkinnedMeshRenderer[] meshes;
    public Color unselectedColour;

    void Start()
    {
        HideSpoon();
        StartCoroutine(RandomWave());
        Select(0);
    }

    // Pass in the charNum in the inpsector for the button
    public void Select(int charNum)
    {
        for (int i = 0; i < m_Animator.Length; i++ )
        {
            if(i == charNum)
            {
                m_Animator[i].SetBool(triggerNames[0], true);
                meshes[i].material.color = Color.white;
            }
            else
            {
                m_Animator[i].SetBool(triggerNames[0], false);
                meshes[i].material.color =  unselectedColour;
            }
        }
        CharacterSelectScreen.Instance.SelectCharacter(charNum);
    }

    public IEnumerator RandomWave()
    {
        randomTime = Random.Range(5.0f, 7.5f);
        int randomChar = Random.Range(0, m_Animator.Length);
        yield return new WaitForSeconds(randomTime);
        for (int i = 0; i < m_Animator.Length; i++)
        {
            if(randomChar == i)
                m_Animator[i].SetTrigger(triggerNames[1]);
        }
        StartCoroutine(RandomWave());
    }

    private void ShowSpoon()
    {
        for (int i = 0; i < spoon.Length; i++)
        {
            spoon[i].SetActive(true);
        }
    }

    private void HideSpoon()
    {
        for (int i = 0; i < spoon.Length; i++)
        {
            spoon[i].SetActive(false);
        }
    }
}
