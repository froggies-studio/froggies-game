using Fighting;
using Movement;
using UnityEngine;

namespace Core.Entities.Player
{
    public interface IPlayerController
    {
        // public MovementInput MovementInput { get; }
        public AttackInfo? AttackInfo { get; }
        public Vector2 Velocity { get; }
        public bool IsDead { get; }
        public bool IsGrounded { get; }
        public bool IsRollingOver { get; }
        public bool IsMoving { get; }
    }
}