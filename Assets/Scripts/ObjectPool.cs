using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int initialSize;
    }

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, GameObject> prefabDictionary;
    private Dictionary<GameObject, string> spawnedObjects = new();

    private void Awake()
    {
        instance = this;

        poolDictionary = new();
        prefabDictionary = new();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new();

            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = CreateNewObject(pool.prefab, pool.tag);
                objectQueue.Enqueue(obj);
            }

            poolDictionary[pool.tag] = objectQueue;
            prefabDictionary[pool.tag] = pool.prefab;
        }
    }

    public GameObject Spawn(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool '{tag}' does not exist!");
            return null;
        }

        Queue<GameObject> queue = poolDictionary[tag];

        if (queue.Count == 0)
        {
            GameObject newObj = CreateNewObject(prefabDictionary[tag], tag);
            queue.Enqueue(newObj);
        }

        GameObject obj = queue.Dequeue();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        spawnedObjects[obj] = tag;
        return obj;
    }

    public void Despawn(GameObject obj, float delay = 0f)
    {
        if (!spawnedObjects.TryGetValue(obj, out string tag))
        {
            Debug.LogWarning("Object does not belong to any pool!");
            return;
        }
        ReturnToPool(tag, obj);
        spawnedObjects.Remove(obj);
    }

    private IEnumerator DespawnRoutine(string tag, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(tag, obj);
    }

    private void ReturnToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        poolDictionary[tag].Enqueue(obj);
    }

    private GameObject CreateNewObject(GameObject prefab, string tag)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.name = tag;
        obj.SetActive(false);
        return obj;
    }
}