using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu]
    public class HitVisualisationData : ScriptableObject
    {
        [SerializeField] private bool isEnabled;
        [SerializeField] private Color bloodColor;
        
        public bool IsEnabled => isEnabled;
        public Color BloodColor => bloodColor;
    }
}