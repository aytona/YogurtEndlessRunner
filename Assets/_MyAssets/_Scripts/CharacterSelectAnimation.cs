using UnityEngine;
using System.Collections;

public class CharacterSelectAnimation : MonoBehaviour {
    private Animator m_Animator;
    private string[] triggerNames = { "Selected", "Wave" };
    public GameObject spoon;

    private float randomTime;

    private bool isSelected = false;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        HideSpoon();
        StartCoroutine(RandomWave());
    }

    public void Select()
    {
        isSelected = !isSelected;
        m_Animator.SetBool(triggerNames[0], isSelected);
    }

    private IEnumerator RandomWave()
    {
        randomTime = Random.Range(5.0f, 7.5f);
        yield return new WaitForSeconds(randomTime);
        m_Animator.SetTrigger(triggerNames[1]);
        StartCoroutine(RandomWave());
    }

    private void ShowSpoon()
    {
        spoon.SetActive(true);
    }

    private void HideSpoon()
    {
        spoon.SetActive(false);
    }
}
