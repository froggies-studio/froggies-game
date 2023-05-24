using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace WaveSystem
{
    public class DayNightVisuals : MonoBehaviour
    {
        [SerializeField] private Light2D _sun;
        [SerializeField] private Gradient _dayGradient;
        [SerializeField] private Gradient _nightGradient;
        
        [SerializeField] private AnimationCurve _dayIntensityCurve;
        [SerializeField] private AnimationCurve _nightIntensityCurve;

        [SerializeField] private float ninthTransition = 10;
        
        public void UpdateVisuals(float t, bool isDay)
        {
            _sun.color = isDay ? _dayGradient.Evaluate(t) : _nightGradient.Evaluate(t);
            _sun.intensity = isDay ? _dayIntensityCurve.Evaluate(t) : _nightIntensityCurve.Evaluate(t);
        }
        
        public void TransitionToNight()
        {
            float nightProgress = 0.0f;
            DOTween.To(() => nightProgress, x => nightProgress = x, 1.0f, nightProgress).OnUpdate(() =>
            {
                _sun.color = _nightGradient.Evaluate(nightProgress);
                _sun.intensity = _nightIntensityCurve.Evaluate(nightProgress);
            });
        }
    }
}