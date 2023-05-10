using UnityEngine;

namespace Core.Entities.Data
{
    public class EnemyDataComponent : MonoBehaviour
    {
        [SerializeField] private EnemyData data;

        public EnemyData Data => data;
    }
}