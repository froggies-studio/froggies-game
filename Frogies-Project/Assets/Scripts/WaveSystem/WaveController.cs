using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using StatsSystem.Health;
using UnityEngine;
using UnityEngine.Video;
using Object = UnityEngine.Object;

namespace WaveSystem
{
    public class WaveController
    {
        public event Action OnWaveCleared;
        public event Action OnWaveStarted;
        public event Action OnEnemyKilled;

        private Dictionary<GameObject, bool> _enemyDictionary;
        private List<GameObject> _spawners;
        private List<GameObject> _enemies;
        private Dictionary<Wave, Wave> _availableWaves;
        private Wave _currentWave;
        private GameObject _currentWaveSpawner;
        private bool _isNight;
        private int _currentAmountOfEnemies;

        public WaveController(Dictionary<Wave, Wave> availableWaves, List<GameObject> spawners, List<GameObject> enemies)
        {
            _spawners = spawners;
            _enemies = enemies;
            _availableWaves = availableWaves;
        }

        public void OnPotionPicked(int numberOfPotions)
        {
            _isNight = true;
            _currentWave = GetWave(numberOfPotions);
            SpawnEnemies();
            if (OnWaveStarted != null) OnWaveStarted.Invoke();
        }

        private Wave GetWave(int numberOfEnemy)
        {
            return _availableWaves.FirstOrDefault(wave => (int)wave.Key.EnemyType == numberOfEnemy).Key;
        }

        public int GetAmountOfEnemies()
        {
            return _currentAmountOfEnemies;
        }

        private void SpawnEnemies()
        {
             _currentWaveSpawner = _spawners[_currentWave.Difficulty];
            var currentEnemy = _enemies[(int)_currentWave.EnemyType];
            for (int i = 0; i < _currentWave.MaxAmountOfEnemies; i++)
            {
                GameObject enemy = Object.Instantiate(currentEnemy.gameObject, _currentWaveSpawner.transform.position,
                    _currentWaveSpawner.transform.rotation, _currentWaveSpawner.transform);
                BasicEnemy createdEnemy = enemy.gameObject.GetComponent<BasicEnemy>();
                createdEnemy.HealthSystem.OnDead += EnemyDeath;
            }
            _currentAmountOfEnemies = _currentWave.MaxAmountOfEnemies;
        }

        public void EnemyDeath(object sender, EventArgs e)
        {
            _currentAmountOfEnemies -= 1;
            if (OnEnemyKilled != null) OnEnemyKilled.Invoke();
            
        }

        public void EnemyChecker()
        {
            if (_isNight)
            {
                _currentAmountOfEnemies = _currentWaveSpawner.transform.childCount;
                if (_currentAmountOfEnemies == 0 && OnWaveCleared!=null)
                {
                    OnWaveCleared.Invoke();
                    _isNight = false;
                }
            }
        }
    }
}
