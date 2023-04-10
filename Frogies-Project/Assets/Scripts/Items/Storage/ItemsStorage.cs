using System.Collections.Generic;
using Items.Scriptable;
using UnityEngine;

namespace Items.Storage
{
    [CreateAssetMenu(fileName = "ItemsStorage", menuName = "ItemsSystem/ItemsStorage")]
    public class ItemsStorage : ScriptableObject
    {
        [field: SerializeField] private List<BaseItemScriptable> ItemScriptables { get; set; }
    }
}