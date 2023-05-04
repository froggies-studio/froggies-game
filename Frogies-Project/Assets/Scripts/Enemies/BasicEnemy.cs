﻿using System.Linq;
using Animation;
using Core;
using Core.Player;
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
        [SerializeField] private DirectionalMover _mover;
        [SerializeField] private MovementData _movementData;
        
        public StatsStorage statsStorage;
        public AnimationStateManager AnimationState;
        public Transform SpriteFlipper;
        public Transform Player;
        private StatsController _statsController;
        private HealthSystem _healthSystem;
        private EnduranceSystem _enduranceSystem;

         public AttacksData attacksData;

        private EnemyMovementInput _inputMoveProvider;
        private EnemyInputFightingProvider _inputFightingInputProvider;
        private BasicAttacker _attacker;
        private PlayerBrain _brain;
        private PlayerAnimationController _animation;


        private void Awake()
        {
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            _healthSystem = new HealthSystem(_statsController);
            _enduranceSystem = new EnduranceSystem(_statsController);
            _inputMoveProvider = new EnemyMovementInput(Player, this.transform);
            _attacker = new BasicAttacker(_enduranceSystem);
            AnimationState.AnimationPerformed += OnAnimationPerformed;
            _inputFightingInputProvider = new EnemyInputFightingProvider();

            _animation = new PlayerAnimationController(AnimationState, SpriteFlipper);
            
            _brain = new PlayerBrain(_movementData, attacksData, _inputMoveProvider, _inputFightingInputProvider, _mover, _animation, statsStorage);
        }

        private void Update()
        {
            _inputFightingInputProvider.CalculateAttackInput(IsInAttackRange);
            _inputMoveProvider.CalculateHorizontalInput(IsInAttackRange);
            _brain.Update();
        }

        private float _attackRange = 1.5f;
        private bool IsInAttackRange => Mathf.Abs(Player.transform.position.x-transform.position.x) < _attackRange;

        private void FixedUpdate()
        {
            _brain.FixedUpdate();
        }


        private void OnAnimationPerformed(PlayerAnimationState animationState)
        {
            switch (animationState)
            {
                case PlayerAnimationState.Attack:
                    Debug.Log("Attack performed");
                    return;
            }
        }
    }
    
}