using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

    public enum PlayerStates
    {
        Run,
        Surf,
        Jump,
        LeftBump,
        RightBump,
        Caught,
        Grounded
    }

    [SerializeField]
    private Animator m_Animator;

    private string[] triggerNames = { "isRunning", "isSurfing", "jump", "leftBump", "rightBump", "isCaught", "isGrounded" };

	void Start () {
        m_Animator = GetComponent<Animator>();
	}
	
    public void Play(PlayerStates animation)
    {
        int num = (int)animation;
        m_Animator.SetTrigger(triggerNames[num]);
    }
}
