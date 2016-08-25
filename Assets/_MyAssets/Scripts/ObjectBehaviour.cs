using UnityEngine;
using System.Collections;

public class ObjectBehaviour : MonoBehaviour {

    #region

    private RandomObjectSet _randomObjectSet;

    //private ItemGenerator _spawnPool;

    #endregion

    #region Monobehaviour

    void Start()
    {
        _randomObjectSet = GetComponent<RandomObjectSet>();
        //_spawnPool = FindObjectOfType<ItemGenerator>();
    }

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
        _randomObjectSet.ResetObjectSet();
        gameObject.SetActive(false);
        //_spawnPool.spawns.Add(this.gameObject);
    }

    #endregion Private Methods
}
