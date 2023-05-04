using System;
using System.Linq;
using Animation;
using Core;
using Core.Player;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using StatsSystem.Health;
using UnityEngine;

namespace Enemies
{
    public class BasicEnemy : MonoBehaviour
    {
        public BasePrefabsStorage prefabsStorage;
        public AnimationStateManager AnimationState;
        public Transform SpriteFlipper;
        public Transform Player;
        private StatsController _statsController;
        private HealthSystem _healthSystem;
        private EnduranceSystem _enduranceSystem;

        [SerializeField] private MovementData _movementData;
        private AttacksData _attacksData;

        private EnemyMovementInput _inputMoveProvider;
        private IFightingInputProvider _inputFightingInputProvider;
        [SerializeField] private DirectionalMover _mover;
        private BasicAttacker _attacker;
        private PlayerBrain _brain;
        private PlayerAnimationController _animation;


        private void Awake()
        {
            var statsStorage = prefabsStorage.StatsStorage;
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            _healthSystem = new HealthSystem(_statsController);
            _enduranceSystem = new EnduranceSystem(_statsController);
            _inputMoveProvider = new EnemyMovementInput(Player, this.transform);
            _attacker = new BasicAttacker(_enduranceSystem);
            AnimationState.AnimationPerformed += OnAnimationPerformed;
            _inputFightingInputProvider = new EnemyInputFightingProvider();

            _animation = new PlayerAnimationController(AnimationState, SpriteFlipper);
            
            _brain = new PlayerBrain(_movementData, _attacksData, _inputMoveProvider, _inputFightingInputProvider, _mover, _animation, statsStorage);
        }

        private void Update()
        {
            _inputMoveProvider.CalculateHorizontalInput();
            _brain.Update();
        }

        private void FixedUpdate()
        {
            _brain.FixedUpdate();
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