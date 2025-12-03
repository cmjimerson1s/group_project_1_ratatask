using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Object_Pooling : MonoBehaviour
{
    [SerializeField] public List<GameObject> pooledObjects = new List<GameObject>();
    public bool spawning = true;
    [SerializeField] int poolRefillAmount;
    public GameObject prefab;

    void Start()
    {
        if (spawning) TopUpPool();
    }

    public GameObject GetPooledObject()
    {
        if (prefab != null && spawning)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
                else if (i == pooledObjects.Count - 1)
                {
                    TopUpPool();
                    i = 0;
                }
            }
        }

        return null;
    }

    public void TopUpPool()
    {
        if (prefab != null && spawning)
        {
            for (int i = 0; i < poolRefillAmount; i++)
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
}