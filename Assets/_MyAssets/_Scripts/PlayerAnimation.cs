using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    public enum PlayerStates
    {
        Run,
        Jump,
        LeftBump,
        RightBump,
        Surf,
        Caught,
        Grounded,
    }

    [SerializeField]
    private Animator m_Animator;

    private string[] triggerNames = { "isRunning", "jump", "leftBump", "rightBump", "isSurfing", "isCaught", "isGrounded" };

    public GameObject spoon;

	void Start () {
        m_Animator = GetComponent<Animator>();
        HideSpoon();
	}
	
    public void Play(PlayerStates animation)
    {
        int num = (int)animation;
        //Debug.Log(triggerNames[num]);
        m_Animator.SetTrigger(triggerNames[num]);
        if (num == 0 || num == 5)
        {
            HideSpoon();
        }
        else if(num == 4)
        {
            ShowSpoon();
        }
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
