using Core.ObjectPoolers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fighting
{
    public class DamageVisuals
    {
        private ParticleSystem _hitEffect;
        private ParticleSystem _decalEffect;
        private Bounds _effectBounds;
        private Transform _root;
        
        private DamageVisualsData _data;

        private bool _returned;
        
        public DamageVisuals(Transform root, Bounds effectBounds, DamageVisualsData data)
        {
            _root = root;
            _hitEffect = ObjectPooler.Instance.SpawnFromPool(ObjectPoolTags.HIT_BLOOD_PARTICLE_EFFECTS)
                .GetComponent<ParticleSystem>();
            _decalEffect = ObjectPooler.Instance.SpawnFromPool(ObjectPoolTags.DECAL_BLOOD_PARTICLE_EFFECTS)
                .GetComponent<ParticleSystem>();
            _effectBounds = effectBounds;
            _data = data;
            
            var hitEffectMain = _hitEffect.main;
            var minMaxGradient = hitEffectMain.startColor;
            minMaxGradient.color = _data.BloodColor;
            hitEffectMain.startColor = minMaxGradient;
            
            var decalEffectMain = _decalEffect.main;
            minMaxGradient = decalEffectMain.startColor;
            minMaxGradient.color = _data.BloodColor;
            decalEffectMain.startColor = minMaxGradient;
        }
        
        public void TriggerEffect(DamageInfo damage)
        {
            if(_returned)
                return;
            
            var offset = new Vector3(Random.Range(_effectBounds.min.x, _effectBounds.max.x), Random.Range(_effectBounds.min.y, _effectBounds.max.y), 0.0f);
            
            _hitEffect.transform.position = _root.position + offset;
            _hitEffect.transform.right = damage.KnockbackInfo.KnockbackDirection;
            _hitEffect.Play();
            
            _decalEffect.transform.position = _root.position + offset;
            _decalEffect.transform.right = damage.KnockbackInfo.KnockbackDirection;
            _decalEffect.Play();
        }

        public void Return()
        {
            if (_returned)
                return;
            
            _returned = true;

            ObjectPooler.Instance.Return(ObjectPoolTags.HIT_BLOOD_PARTICLE_EFFECTS, _hitEffect.gameObject);
            ObjectPooler.Instance.Return(ObjectPoolTags.DECAL_BLOOD_PARTICLE_EFFECTS, _decalEffect.gameObject);
        }
        
        public struct DamageVisualsData
        {
            public Color BloodColor { get; set; }
        }
    }
}