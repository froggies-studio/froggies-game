using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using StatsSystem.Endurance;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Movement
{
    public class DirectionalMover : MonoBehaviour
    {
        [Header("COLLISION")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private int _detectorCount = 3;
        [SerializeField] private float _detectionRayLength = 0.1f;
        [SerializeField] [Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f;
        [SerializeField] private bool debugGroundChecks = false;
        
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _rollOverBuffer = 0.1f;
        
        [SerializeField] private float _jumpEndEarlyGravityMultiplier = 0.45f;
        
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private new Collider2D collider;
        
        [SerializeField]private bool isDashing = false;
        private float _rollOverStartTime;
        private float _rollOverDuration;

        public Vector2 Velocity => rigidbody.velocity;
        public bool IsGrounded => _collisionGround;
        public bool IsDashing => isDashing;
        public float RollOverStartTime => _rollOverStartTime;
        public float RollOverDuration => _rollOverDuration;
        
        private bool _collisionGround;
        private float _ofGroundTime;
        private bool _coyoteUsable;
        private bool _endedJumpEarly = true;
        private float _apexPoint;
        private float _lastJumpPressed = -100f;
        private float _lastRollOverPressed = -100f;
        private float _originalVelocity;
        private FacingDirection _facingDirection = FacingDirection.Right;

        private bool CanUseCoyote => !_collisionGround && _coyoteUsable && _ofGroundTime + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => _collisionGround && _lastJumpPressed + _jumpBuffer > Time.time;
        private bool HasBufferedRollOver => _collisionGround && _lastRollOverPressed + _rollOverBuffer > Time.time;
        
        #region Collisions
    
        public void RunGroundCheck()
        {
            Vector2 minBounds = (Vector2) collider.bounds.min + collider.offset + Vector2.up * .05f;
            Vector2 maxBounds = (Vector2) collider.bounds.max + collider.offset + Vector2.up * .05f;
            
            var groundedCheck = EvaluateRayPositions(new Vector2(minBounds.x + _rayBuffer, minBounds.y), new Vector2(maxBounds.x - _rayBuffer, minBounds.y))
                .Any(point => Physics2D.Raycast(point, Vector2.down, _detectionRayLength, _groundLayer));;
        
            switch (_collisionGround)
            {
                case true when !groundedCheck:
                    _ofGroundTime = Time.time;
                    break;
                case false when groundedCheck:
                    _coyoteUsable = true;
                    break;
            }

            _collisionGround = groundedCheck;
        }
    
        private IEnumerable<Vector2> EvaluateRayPositions(Vector2 start, Vector2 end)
        {
            for (var i = 0; i < _detectorCount; i++)
            {
                var t = (float) i / (_detectorCount - 1);
                if(debugGroundChecks)
                    Debug.DrawRay(Vector2.Lerp(start, end, t), Vector2.down * _detectionRayLength);
                
                yield return Vector2.Lerp(start, end, t);
            }
        }
    
        #endregion
    
        #region Walk
    
        public void CalculateHorizontalSpeed(MovementInput input, MovementData data)
        {
            if (isDashing)
            {
                return;
            }
            float currentHorizontalSpeed = rigidbody.velocity.x;
            if (input.X != 0)
            {
                currentHorizontalSpeed += input.X * data.Acceleration * Time.deltaTime;

                currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, -data.MoveClamp, data.MoveClamp);
                
                _facingDirection = currentHorizontalSpeed > 0 ? FacingDirection.Right : FacingDirection.Left;
            }
            else
            {
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, data.DeAcceleration * Time.deltaTime);
            }
            
            var rigidbodyVelocity = rigidbody.velocity;
            rigidbodyVelocity.x = currentHorizontalSpeed;
            rigidbody.velocity = rigidbodyVelocity;
        }

        public void Stop()
        {
            var rigidbodyVelocity = rigidbody.velocity;
            rigidbodyVelocity.x = 0;
            rigidbody.velocity = rigidbodyVelocity;
        }

        #endregion
    
        #region Jump
    
        public void CalculateJump(MovementInput input, MovementData data, EnduranceSystem enduranceSystem)
        {
            if (!enduranceSystem.CheckEnduranceAbility(data.AmountOfEnduranceToJump))
            {
                return;
            }
            float currentVerticalSpeed = rigidbody.velocity.y;
            
            if (currentVerticalSpeed < data.FallClamp) 
                currentVerticalSpeed = data.FallClamp;

            if (input.JumpDown)
                _lastJumpPressed = Time.time;
            
            if (input.JumpDown && CanUseCoyote || HasBufferedJump)
            {
                enduranceSystem.UseEndurance(data.AmountOfEnduranceToJump);
                currentVerticalSpeed = data.JumpVelocity;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _ofGroundTime = float.MinValue;
            }

            // End the jump early if button released
            if (!_collisionGround && input.JumpUp && !_endedJumpEarly && rigidbody.velocity.y > 0)
            {
                currentVerticalSpeed *= _jumpEndEarlyGravityMultiplier;
                _endedJumpEarly = true;
            }
            
            var rigidbodyVelocity = rigidbody.velocity;
            rigidbodyVelocity.y = currentVerticalSpeed;
            rigidbody.velocity = rigidbodyVelocity;
        }

        #endregion
        
        #region RollOver

        public void CalculateRollOver(MovementInput input, MovementData data, EnduranceSystem enduranceSystem)
        {
            if (input.RollOver)
            {
                _lastRollOverPressed = Time.time;
            }
            if (!enduranceSystem.CheckEnduranceAbility(data.AmountOfEnduranceToRollOver) || !HasBufferedRollOver)
            {
                return;
            }

            isDashing = true;
            if (input.RollOver)
            {
                enduranceSystem.UseEndurance(data.AmountOfEnduranceToRollOver);
                _rollOverStartTime = Time.fixedTime;
                _rollOverDuration = data.DashDuration;
                if (rigidbody.velocity.normalized.x != 0)
                {
                    var velocity = rigidbody.velocity;
                    _originalVelocity = velocity.x;
                    var acceleration = data.RollOverMovingVelocity * velocity.normalized.x;
                    velocity = new Vector2(velocity.x + acceleration, velocity.y);
                    rigidbody.velocity = velocity;
                }
                else
                {
                    rigidbody.AddForce(new Vector2(data.RollOverStayingVelocity*(int)_facingDirection, 0));
                }
            }
        }

        public void EndRollOver()
        {
            isDashing = false;
            rigidbody.velocity = new Vector2(_originalVelocity, rigidbody.velocity.y);
        }
        #endregion
    }
}