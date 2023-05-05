using Items.Data;
using UnityEngine;

namespace Items.Scriptable
{
    [CreateAssetMenu(fileName = "Item", menuName = "ItemsSystem/StatChangingItem")]
    public class StatChangingItemScriptable : BaseItemScriptable
    {
        [SerializeField] private StatChangingItemDescriptor _itemDescriptor;
        public override ItemDescriptor ItemDescriptor => _itemDescriptor;
    }
}