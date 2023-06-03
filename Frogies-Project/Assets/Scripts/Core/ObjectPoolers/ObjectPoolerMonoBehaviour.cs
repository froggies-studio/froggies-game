using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.ObjectPoolers
{
    public class ObjectPoolerMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject pooledObjectPrefab;
        [SerializeField] private int size;
        [SerializeField] private float delay;

        private void Start()
        {
            InitObjectPooler();
        }

        private void InitObjectPooler()
        {
            ObjectPooler.Instance.AddPooler(new ObjectPooler.Pool()
            {
                Parent = Instantiate(new GameObject(), transform),
                Prefab = pooledObjectPrefab,
                Tag = pooledObjectPrefab.name,
                Size = size
            });

            StartCoroutine(SpawnLoopWithDelay());
        }

        private IEnumerator SpawnLoopWithDelay()
        {
            for (int i = 0; i < 1000; i++)
            {
                Vector3 position = new Vector3(Random.Range(-0.7f, 15f), 10, 0);
                ObjectPooler.Instance.SpawnFromPool(pooledObjectPrefab.name, position, Quaternion.identity);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}