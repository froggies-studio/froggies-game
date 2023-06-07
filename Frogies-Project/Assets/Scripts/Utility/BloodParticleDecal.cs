using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility
{
    public class BloodParticleDecal : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private GameObject spriteParticle;
        [SerializeField] private GameObject spriteDecal;
        
        [SerializeField] private Vector2 activationDelayRange;
        [SerializeField] private Vector2 speedRange;

        private float _activationTimer;
        private bool _isDecalActive;
        
        public void Initialise(Vector2 direction)
        {
            spriteParticle.SetActive(true);
            spriteDecal.SetActive(false);
            
            rigidbody2D.velocity = direction * Random.Range(speedRange.x, speedRange.y);
            _activationTimer = Random.Range(activationDelayRange.x, activationDelayRange.y);
        }

        private void Update()
        {
            _activationTimer -= Time.deltaTime;
            if(_activationTimer < 0 && !_isDecalActive)
                SwitchToBloodDecal();
        }
        
        private void SwitchToBloodDecal()
        {
            _isDecalActive = true;
            spriteParticle.SetActive(false);
            spriteDecal.SetActive(true);
            
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            rigidbody2D.Sleep();
        }
    }
}