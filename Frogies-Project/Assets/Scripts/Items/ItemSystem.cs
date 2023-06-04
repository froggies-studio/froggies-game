using System.Collections.Generic;
using Core;
using Core.InventorySystem;
using Items.Behaviour;
using Items.Core;
using Items.Data;
using Items.Enum;
using Items.Rarity;
using UnityEngine;

namespace Items
{
    public class ItemSystem
    {
        private static readonly int RarityCount = System.Enum.GetValues(typeof(ItemRarity)).Length;
        
        private readonly SceneItem _sceneItem;
        private readonly Transform _transform;
        private readonly Dictionary<SceneItem, Item> _itemsOnScene;
        private readonly IItemRarityColor[] _rarityColors;
        private readonly ItemFactory _itemFactory;
        private readonly Inventory _inventory;


        public ItemSystem(SceneItem sceneItemPrefab, IItemRarityColor[] colors, ItemFactory itemFactory, Inventory inventory)
        {
            Debug.Assert(colors.Length == RarityCount, "Rarity count is not equal to colors count");
            
            _sceneItem = sceneItemPrefab;
            _itemsOnScene = new Dictionary<SceneItem, Item>();
            GameObject gameObject = new GameObject(nameof(ItemSystem));
            _transform = gameObject.transform;
            _rarityColors = colors;
            _itemFactory = itemFactory;
            _inventory = inventory;
        }

        public void DropItem(ItemDescriptor descriptor, Vector2 position)
        {
            var item = _itemFactory.CreateItem(descriptor);
            DropItem(item, position);
        }

        private void DropItem(Item item, Vector2 position)
        {
            var sceneItem = Object.Instantiate(_sceneItem, _transform);
            string itemName = item.Descriptor.ItemId.ToString();
            var rarityColor = _rarityColors[(int) item.Descriptor.ItemRarity].Color;
            sceneItem.SetItem(item.Descriptor.ItemSprite, itemName, rarityColor);
            sceneItem.DropItem(position);
            sceneItem.ItemClicked += OnItemClicked;
            _itemsOnScene.Add(sceneItem, item);
        }

        private void OnItemClicked(SceneItem sceneItem)
        {
            TryPickItem(sceneItem);
        }
        
        private void TryPickItem(SceneItem sceneItem)
        {
            Vector2 playerPosition = GlobalSceneManager.Instance.PlayerTransform.position;
            Vector2 itemPosition = sceneItem.transform.position;
            Vector2 distanceVec = playerPosition - itemPosition;
            float distanceSqr = Vector2.SqrMagnitude(distanceVec);
            float interactionDistanceSqr = sceneItem.InteractionDistance * sceneItem.InteractionDistance;
            
            if (distanceSqr > interactionDistanceSqr) 
                return;
            
            var item = _itemsOnScene[sceneItem];
            _itemsOnScene.Remove(sceneItem);
            sceneItem.ItemClicked -= OnItemClicked;
            Object.Destroy(sceneItem.gameObject);
            _inventory.AddNewItem(item);
        }
    }
}