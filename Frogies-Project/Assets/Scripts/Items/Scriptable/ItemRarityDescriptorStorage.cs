using Items.Data;
using UnityEngine;

namespace Items.Scriptable
{
    [CreateAssetMenu(fileName = "ItemRarity", menuName = "ItemsSystem/ItemRarity")]
    public class ItemRarityDescriptorStorage : ScriptableObject
    {
        [field: SerializeField] private RarityDescriptor[] _rarityDescriptor;
        
        public RarityDescriptor[] RarityDescriptor => _rarityDescriptor;
    }
}