using UnityEngine;
using UnityEngine.Serialization;

namespace Movement
{
    [CreateAssetMenu]
    public class MovementData : ScriptableObject
    {
        [Header("WALKING")]
        [SerializeField] private float moveClamp = 13;
        [SerializeField] private float deAcceleration = 60f;
        
        [Header("GRAVITY")] [SerializeField] private float fallClamp = -40f;
        [Header("JUMPING")] 
        [SerializeField] private float amountOfEnduranceToJump = 5;
        
        [Header("ROLLING OVER")]
        [SerializeField] private float amountOfEnduranceToRollOver = 10;
        [SerializeField] private float rollOverMovingVelocity = 2f;
        [SerializeField] private float rollOverStayingVelocity = 100f;
        [SerializeField] private float dashDuration = 0.2f;  
        
        public float MoveClamp => moveClamp;
        public float DeAcceleration => deAcceleration;
        public float FallClamp => fallClamp;
        public float AmountOfEnduranceToJump => amountOfEnduranceToJump;
        public float AmountOfEnduranceToRollOver => amountOfEnduranceToRollOver;
        public float RollOverMovingVelocity => rollOverMovingVelocity;
        public float RollOverStayingVelocity => rollOverStayingVelocity;
        public float DashDuration => dashDuration;
    }
}