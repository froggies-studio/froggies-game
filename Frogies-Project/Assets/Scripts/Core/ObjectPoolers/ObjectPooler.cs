using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.ObjectPoolers
{
    public class ObjectPooler
    {
        public static ObjectPooler Instance => _instance ??= new ObjectPooler();
        private static ObjectPooler _instance;
        
        private Dictionary<string, Queue<GameObject>> _poolDictionary;

        
        public ObjectPooler()
        {
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();
        }

        public void AddPooler(Pool pool)
        {
            var objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                var obj = pool.Parent != null 
                    ? Object.Instantiate(pool.Prefab, pool.Parent.transform) 
                    : Object.Instantiate(pool.Prefab);
                    
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.Tag, objectPool);
        }

        public void AddOrUpdatePooler(Pool pool)
        {
            if (_poolDictionary.ContainsKey(pool.Tag))
            {
                _poolDictionary.Remove(pool.Tag);
            }
            
            var objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                var obj = pool.Parent != null 
                    ? Object.Instantiate(pool.Prefab, pool.Parent.transform) 
                    : Object.Instantiate(pool.Prefab);
                    
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.Tag, objectPool);
        }
        
        public GameObject SpawnFromPool(string objectPoolTag)
        {
            return SpawnFromPool(objectPoolTag, Vector3.zero, Quaternion.identity);
        }
        
        public GameObject SpawnFromPool(string objectPoolTag, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.TryGetValue(objectPoolTag, out var objectPool))
            {
                Debug.LogWarning($"Pool with tag {objectPoolTag} doesn't exist");
                return null;
            }

            //TODO: Add dynamic pool resizing
            
            var objectToSpawn = objectPool.Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            
            objectToSpawn.GetComponent<IPooledObject>()?.OnObjectSpawn();
            
            return objectToSpawn;
        }
        
        public void Return(string objectPoolTag, GameObject gameObject)
        {
            if (!_poolDictionary.TryGetValue(objectPoolTag, out var objectPool))
            {
                Debug.LogWarning($"Pool with tag {objectPoolTag} doesn't exist");
                return;
            }
            
            objectPool.Enqueue(gameObject);
        }

        [Serializable]
        public struct Pool
        {
            public string Tag { get; set; }
            public GameObject Prefab { get; set; }
            public int Size { get; set; }
            [CanBeNull] public GameObject Parent { get; set; }
        }
    }
}