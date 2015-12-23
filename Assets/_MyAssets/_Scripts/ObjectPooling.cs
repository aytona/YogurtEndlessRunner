using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Note: For future use, and reference
public class ObjectPooling : MonoBehaviour {

    public static ObjectPooling current;

    #region Variables

    public GameObject pooledObject;
    private int pooledAmount;
    private bool willGrow = true;
    private List<GameObject> pooledObjects;

    #endregion Variables

    #region Monobehaviour

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    #endregion Monobehaviour

    #region Public Methods

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
        }

        if (willGrow)
        {
            GameObject obj = Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }

    #endregion Public Methods
}
