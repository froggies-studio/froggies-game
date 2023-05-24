using System;
using Fighting;
using Movement;
using Unity.VisualScripting;
using UnityEngine;

namespace Animation
{
    public class PlayerAnimationController
    {

        private readonly AnimationStateManager _animationStateManager;
        private readonly Transform _animationFlipper;

        public PlayerAnimationController(AnimationStateManager animationStateManager, Transform animationFlipper)
        {
            _animationStateManager = animationStateManager;
            _animationFlipper = animationFlipper;
        }
        
        public event Action<PlayerAnimationState> AnimationPerformed
        {
            add => _animationStateManager.AnimationPerformed += value;
            remove => _animationStateManager.AnimationPerformed -= value;
        }


        public void UpdateAnimationSystem(MovementInput input, AttackInfo? attackInfo, Vector2 velocity,
            bool moverIsGrounded, bool isDead, bool isRollingOver)
        {
            PlayerAnimationState newState = PlayerAnimationState.Idle;
           
            bool isTurning = false;

            if (isDead)
            {
                _animationStateManager.TriggerAnimationState(PlayerAnimationState.Death);
                return;
            }
            
            if (attackInfo.HasValue)
            {
                _animationStateManager.TriggerAnimationState(attackInfo.Value.animationState);
                return;
            }

            if (velocity.y != 0 && !moverIsGrounded)
            {
                newState = velocity.y > 0 ? PlayerAnimationState.Jump : PlayerAnimationState.Fall;
            }
            else if (velocity.x != 0)
            {
                isTurning = (velocity.x * input.X) < 0;
                newState = isTurning ? PlayerAnimationState.QuickTurn : PlayerAnimationState.Run;
                if (isRollingOver)
                {
                    newState = PlayerAnimationState.RollOver;
                }
            }
        
            var animationFlipperLocalScale = _animationFlipper.localScale;
            if (Mathf.Sign(animationFlipperLocalScale.x) != Mathf.Sign(input.X) && input.X != 0 && !isTurning)
            {
                animationFlipperLocalScale.x = Mathf.Sign(input.X);
                _animationFlipper.localScale = animationFlipperLocalScale;
            }
            
            _animationStateManager.ApplyAnimationState(newState);
        }
    }
}