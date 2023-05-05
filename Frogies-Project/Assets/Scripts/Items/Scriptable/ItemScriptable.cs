using Items.Data;
using UnityEngine;

namespace Items.Scriptable
{
    [CreateAssetMenu(fileName = "Item", menuName = "ItemsSystem/Item")]
    public class ItemScriptable : BaseItemScriptable
    {
        [SerializeField] private ItemDescriptor _itemDescriptor;
        public override ItemDescriptor ItemDescriptor => _itemDescriptor;
        
    }
}