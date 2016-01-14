using UnityEngine;
using System.Collections;

public class ObjectBehaviour : MonoBehaviour {

    #region Monobehaviour

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * GameManager.Instance.gameSettings.gameSpeed);
        if (transform.position.x <= GameManager.Instance.lengthBeforeDespawn || !GameManager.Instance.gameSettings.gameStart)
        {
            DestroyObjectSet();
        }
    }

    #endregion Monobehaviour

    #region Private Methods

    private void DestroyObjectSet()
    {
        gameObject.SetActive(false);
    }

    #endregion Private Methods
}
