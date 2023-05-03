using System;
using System.Linq;
using Animation;
using Core;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using StatsSystem.Health;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    public class BasicEnemy : MonoBehaviour
    {
        public BasePrefabsStorage prefabsStorage;
        public AnimationStateManager AnimationState;
        public Transform SpriteFlipper;
        private HealthSystem _healthSystem;
        private PlayerAnimationController _animationController;
        private IMovementInputProvider _movementInputProvider;
        private StatsController _statsController;
        private EnduranceSystem _enduranceSystem;

        private void Awake()
        {
            var statsStorage = prefabsStorage.StatsStorage;
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            _healthSystem = new HealthSystem(_statsController);
            _enduranceSystem = new EnduranceSystem(_statsController);
            _movementInputProvider = new EnemyMovementInput();

            AnimationState.AnimationPerformed += OnAnimationPerformed;

            _animationController = new PlayerAnimationController(AnimationState, SpriteFlipper);
            _animationController.UpdateAnimationSystem(_movementInputProvider.Input, null, Vector2.zero, true, false);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.K))
            {
                Debug.Log("Key pressed");
                var testAttackInfo = new AttackInfo() { animationState = PlayerAnimationState.Attack };
                _animationController.UpdateAnimationSystem(_movementInputProvider.Input, testAttackInfo, Vector2.zero, true,
                    false);
            }
        }

        private void OnAnimationPerformed(PlayerAnimationState animationState)
        {
            switch (animationState)
            {
                case PlayerAnimationState.Attack:
                    Debug.Log("I am attacking");
                    return;
            }
        }
    }
}