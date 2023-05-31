using Core.Entities.Data;
using Core.Entities.Enemies;
using UnityEngine;

namespace Core.Entities.Spawners
{
    public class EnemySpawner : ISpawner
    {
        private static int _enemiesCounter;
        private readonly GlobalSceneManager _globalSceneManager;

        public EnemySpawner(GlobalSceneManager globalSceneManager)
        {
            _globalSceneManager = globalSceneManager;
        }

        public BasicEntity Spawn(GameObject enemyPrefab, out GameObject enemyGameObject)
        {
            enemyGameObject = Object.Instantiate(enemyPrefab);
            var enemyData = enemyGameObject.GetComponent<EnemyDataComponent>();
            enemyData.Data.Player = _globalSceneManager.PlayerData.DirectionalMover.transform;
            enemyData.Data.Renderer.sortingOrder = _enemiesCounter++;
            var basicEnemy = new BasicEnemy(enemyData.Data);
            _globalSceneManager.Entities.Add(basicEnemy);
            return basicEnemy;
        }
    }
}