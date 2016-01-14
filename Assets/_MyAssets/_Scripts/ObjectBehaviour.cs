using UnityEngine;
using System.Collections;

public class ObjectBehaviour : MonoBehaviour {

    #region Variables

    [Tooltip("Length of object lifespan")]
    public float lifeSpan;

    #endregion Variables

    #region Monobehaviour

    void OnEnable()
    {
        Invoke("Destroy", lifeSpan);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);
        if (transform.position.x <= GameManager.Instance.lengthBeforeDespawn || !GameManager.Instance.gameSettings.gameStart)
            DestroyParent();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Destroy();
    }

    #endregion Monobehaviour

    #region Private Methods

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void DestroyParent()
    {
        transform.parent.gameObject.SetActive(false);
    }

    #endregion Private Methods
}
