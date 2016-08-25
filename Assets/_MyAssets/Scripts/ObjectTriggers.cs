using UnityEngine;
using System.Collections;

public class ObjectTriggers : MonoBehaviour
{
    #region MonoBehaviour

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy();
        }
    }

    #endregion MonoBehaviour

    #region Private Methods

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    #endregion Private Methods
}
