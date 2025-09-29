using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }
    
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int initialSize;
    }
    
    public List<Pool> pools;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;
    
    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
        
        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.prefab, objectPool);
        }
    }
    
    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("Pool for prefab " + prefab.name + " doesn't exist.");
            return null;
        }
        
        if (poolDictionary[prefab].Count == 0)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            poolDictionary[prefab].Enqueue(obj);
        }
        
        GameObject objectToSpawn = poolDictionary[prefab].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        
        return objectToSpawn;
    }
    
    public void ReturnToPool(GameObject prefab, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("Pool for prefab " + prefab.name + " doesn't exist.");
            return;
        }
        
        objectToReturn.SetActive(false);
        poolDictionary[prefab].Enqueue(objectToReturn);
    }
}