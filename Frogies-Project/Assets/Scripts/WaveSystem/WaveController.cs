using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Entities.Spawners;
using UnityEngine;

namespace WaveSystem
{
    public class WaveController
    {
        public event Action OnWaveCleared;
        public event Action OnWaveStarted;
        public event Action OnEnemyKilled;
        public event Action OnLastWaveCleared;

        private Dictionary<GameObject, bool> _enemyDictionary;
        private List<GameObject> _spawners;
        private List<GameObject> _enemies;
        private Dictionary<Wave, Wave> _availableWaves;
        private Wave _currentWave;
        private GameObject _currentWaveSpawner;
        private bool _isNight;
        private bool _lastWave;
        private int _currentAmountOfEnemies;
        private EnemySpawner _enemySpawner;

        public WaveController(Dictionary<Wave, Wave> availableWaves,
            List<GameObject> spawners,
            List<GameObject> enemies,
            EnemySpawner enemySpawner)
        {
            _spawners = spawners;
            _enemies = enemies;
            _availableWaves = availableWaves;
            _enemySpawner = enemySpawner;
            OnEnemyKilled += EnemyChecker;
        }

        public void OnPotionPicked(int numberOfPotions, bool lastPotion)
        {
            _isNight = true;
            _currentWave = GetWave(numberOfPotions - 1);
            _lastWave = lastPotion;
            Debug.Assert(_currentWave != null);
            SpawnEnemies();
            GlobalSceneManager.Instance.RespawnPlayer();
            if (OnWaveStarted != null) OnWaveStarted.Invoke();
        }

        private Wave GetWave(int numberOfEnemy)
        {
            return _availableWaves.FirstOrDefault(wave => wave.Key.Difficulty == numberOfEnemy).Key;
        }

        public int GetAmountOfEnemies()
        {
            return _currentAmountOfEnemies;
        }

        private void SpawnEnemies()
        {
            _currentWaveSpawner = _spawners[_currentWave.Difficulty];

            foreach (var enemyTypeCounter in _currentWave.EnemyTypeCounter)
            {
                var currentEnemy = _enemies[(int)enemyTypeCounter.EnemyType];
                for (int i = 0; i < enemyTypeCounter.Amount; i++)
                {
                    var enemy = _enemySpawner.Spawn(currentEnemy, out GameObject newEnemy, _currentWave.Difficulty);
                    newEnemy.transform.position = _currentWaveSpawner.transform.position;
                    newEnemy.transform.parent = _currentWaveSpawner.transform;
                    enemy.Brain.HealthSystem.OnDead += EnemyDeath;
                }
            }

            _currentAmountOfEnemies = _currentWave.MaxAmountOfEnemies;
        }

        public void EnemyDeath(object sender, EventArgs e)
        {
            _currentAmountOfEnemies -= 1;
            OnEnemyKilled?.Invoke();
        }

        public void EnemyChecker()
        {
            if (_isNight && _currentAmountOfEnemies == 0)
            {
                _isNight = false;

                if (_lastWave)
                {
                    OnLastWaveCleared?.Invoke();
                    return;
                }

                OnWaveCleared?.Invoke();
            }
        }
    }
}