using Items.Enum;
using UnityEngine;

namespace Items.Rarity
{
    public interface IItemRarityColor
    {
        ItemRarity Rarity { get; }
        Color Color { get; }
    }
}