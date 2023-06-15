using System;
using Animation;
using UnityEngine;

namespace Core.Entities.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Player player;

        private static readonly int PlayerIdle = Animator.StringToHash("PlayerIdle");
        private static readonly int PlayerRun = Animator.StringToHash("PlayerRun");
        private static readonly int PlayerRollOver = Animator.StringToHash("PlayerRollOver");
        private static readonly int PlayerQuickTurn = Animator.StringToHash("PlayerQuickTurn");
        private static readonly int PlayerJump = Animator.StringToHash("PlayerJump");
        private static readonly int PlayerFall = Animator.StringToHash("PlayerFall");
        private static readonly int PlayerFallInBetween = Animator.StringToHash("PlayerFallInBetween");
        private static readonly int PlayerGetHit = Animator.StringToHash("PlayerGetHit");
        private static readonly int PlayerAttack = Animator.StringToHash("PlayerAttack");
        private static readonly int PlayerAttack2 = Animator.StringToHash("PlayerAttack2");
        private static readonly int PlayerDeath = Animator.StringToHash("PlayerDeath");

        private int _currentState;
        private IPlayerController _playerController;
        private Animator _animator;

        // public void UpdateAnimationSystem(MovementInput input, AttackInfo? attackInfo, Vector2 velocity,
        //     bool moverIsGrounded, bool isDead, bool isRollingOver)
        // {
        //     PlayerAnimationState newState = PlayerAnimationState.Idle;
        //    
        //     bool isTurning = false;
        //
        //     if (isDead)
        //     {
        //         _animationStateManager.TriggerAnimationState(PlayerAnimationState.Death);
        //         return;
        //     }
        //     
        //     if (attackInfo.HasValue)
        //     {
        //         _animationStateManager.TriggerAnimationState(attackInfo.Value.animationState);
        //         return;
        //     }
        //
        //     if (velocity.y != 0 && !moverIsGrounded)
        //     {
        //         newState = velocity.y > 0 ? PlayerAnimationState.Jump : PlayerAnimationState.Fall;
        //     }
        //     else if (velocity.x != 0)
        //     {
        //         isTurning = (velocity.x * input.X) < 0;
        //         newState = isTurning ? PlayerAnimationState.QuickTurn : PlayerAnimationState.Run;
        //         if (isRollingOver)
        //         {
        //             newState = PlayerAnimationState.RollOver;
        //         }
        //     }
        //
        //     var animationFlipperLocalScale = _animationFlipper.localScale;
        //     if (Mathf.Sign(animationFlipperLocalScale.x) != Mathf.Sign(input.X) && input.X != 0 && !isTurning)
        //     {
        //         animationFlipperLocalScale.x = Mathf.Sign(input.X);
        //         _animationFlipper.localScale = animationFlipperLocalScale;
        //     }
        //     
        //     _animationStateManager.ApplyAnimationState(newState);
        // }

        private void Start()
        {
            _playerController = player.GetComponent<IPlayerController>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var state = GetState();
            if (_currentState == state)
            {
                return;
            }

            _animator.CrossFade(state, 0, 0);
            _currentState = state;
        }

        private float _lockedTill;

        private int GetState()
        {
            //Most priority
            if (_playerController.IsDead) return PlayerDeath;

            if (Time.time < _lockedTill) return _currentState;

            if (!_playerController.IsGrounded)
            {
                if (_playerController.Velocity.y <= 0f)
                {
                    return PlayerFall;
                }
                else
                {
                    return PlayerJump;
                }
            }

            if (_playerController.IsMoving)
            {
                return PlayerRun;
            }

            return PlayerIdle;

            int LockState(int s, float t)
            {
                _lockedTill = Time.time + t;
                return s;
            }
        }
    }
}