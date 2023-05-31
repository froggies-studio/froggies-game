using UnityEngine;

namespace Core.Entities.Spawners
{
    public interface ISpawner
    {
        BasicEntity Spawn(GameObject prefab, out GameObject gameObject);
    }
}