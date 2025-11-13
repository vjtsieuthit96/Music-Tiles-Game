using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<string, object> pool = new Dictionary<string, object>();

    private void Awake()
    {
        Instance = this;
    }

    public void CreatePool<T>(string key, T prefab, int initialSize)
        where T : MonoBehaviour
    {
        if (!pool.ContainsKey(key))
        {
            ObjectPoolGeneric<T> objectPool = new ObjectPoolGeneric<T>(prefab, initialSize, transform);
            pool.Add(key, objectPool);
        }
    }
   
    public T GetObject<T>(string key, Vector3 position, Quaternion rotation) where T : MonoBehaviour
    {
        if (pool.ContainsKey(key))
        {
            var objectPool = (ObjectPoolGeneric<T>)pool[key];
            return objectPool.GetObject(position, rotation);
        }
        return null;
    }

    public void ReturnObject<T>(string key, T obj) where T : MonoBehaviour
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("Key truyền vào `ReturnObject()` bị null hoặc rỗng!");
            return;
        }

        if (!pool.ContainsKey(key))
        {
            Debug.LogError($"Pool `{key}` không tồn tại! Đối tượng không thể trả về.");
            return;
        }

        var objectPool = (ObjectPoolGeneric<T>)pool[key];
        objectPool.ReturnObject(obj);
    }
}