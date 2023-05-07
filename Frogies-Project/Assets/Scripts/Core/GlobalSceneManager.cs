using System;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Core.InventorySystem;
using Core.Player;
using Fighting;
using Items;
using Items.Behaviour;
using Items.Core;
using Items.Data;
using Items.Enum;
using Items.Rarity;
using Items.Scriptable;
using Items.Storage;
using JetBrains.Annotations;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using StatsSystem.Health;
using UnityEngine;

namespace Core
{
    public class GlobalSceneManager : MonoBehaviour
    {
        public static GlobalSceneManager Instance { get; private set; }
        public static PlayerInputActions InputInstance => Instance.Input;

        [SerializeField] private MovementData _movementData;
        [SerializeField] private AttacksData _attacksData;
        
        [SerializeField] private new Camera camera;
        [SerializeField] private DirectionalMover player;
        [SerializeField] private AnimationStateManager animationStateManager;
        [SerializeField] private Transform spriteFlipper;
        
        [SerializeField] private ItemsStorage itemsStorage;
        [SerializeField] private BasePrefabsStorage prefabsStorage;
        [SerializeField] private ItemRarityDescriptorStorage itemRarityDescriptor;
        [SerializeField] private PotionSystem.PotionSystem potionSystem;
        [SerializeField] private Inventory inventory;

        [SerializeField] private HealthBar playerHealthBar;
        [SerializeField] private EnduranceControlBar _enduranceControlBar;
        
        public PlayerInputActions Input { get; private set; }
        [CanBeNull] public Camera GlobalCamera { get; set; }
        
        public Transform PlayerTransform => player.transform;
        
        public BasePrefabsStorage PrefabsStorage => prefabsStorage;

        private PlayerBrain _playerBrain;
        private ItemSystem _sceneItemStorage;
        private DropGenerator _dropGenerator;

        private bool isPaused = false;

        private StatsStorage statsStorage;
        
        private void Awake()
        {
            Debug.Assert(Instance == null);
            Instance = this;

            GlobalCamera = camera;
            Input = new PlayerInputActions();
            Input.Enable();

            PlayerMoveInputReader moveInputReader = new PlayerMoveInputReader(Input);
            PlayerFightInputReader fightInputReader = new PlayerFightInputReader(Input, _attacksData);
            PlayerAnimationController playerAnimation = new PlayerAnimationController(animationStateManager, spriteFlipper);
            BasicAttacker attacker = new BasicAttacker();
            statsStorage = prefabsStorage.StatsStorage;
            _playerBrain = new PlayerBrain(_movementData, _attacksData, moveInputReader, fightInputReader, player, attacker, playerAnimation, statsStorage);
            playerHealthBar.Setup(_playerBrain.StatsController);
            _enduranceControlBar.Setup(_playerBrain.StatsController);

            ItemFactory factory = new ItemFactory(_playerBrain.StatsController);
            _sceneItemStorage = new ItemSystem(
                PrefabsStorage.SceneItemPrefab.GetComponent<SceneItem>(), 
                itemRarityDescriptor.RarityDescriptor.Cast<IItemRarityColor>().ToArray(), 
                factory, inventory);
            
            var descriptors = itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            _dropGenerator = new DropGenerator(player, _sceneItemStorage, descriptors);

            var depowerPotions = descriptors.Where(descriptor => descriptor.ItemId == ItemId.DepowerPotion)
                .Select(descriptor => new Potion(descriptor as StatChangingItemDescriptor, _playerBrain.StatsController)).ToList();
            potionSystem.Setup(depowerPotions);
            potionSystem.OnActive += () => isPaused = true;
            potionSystem.OnOptionSelected += () => isPaused = false;
        }

        private void Update()
        {
            if(isPaused)
                return;
            
            // TODO: remove
            if (UnityEngine.Input.GetKeyUp(KeyCode.P)) // for testing purpose only
            {
                potionSystem.OpenPotionMenu();
            }
            
            _dropGenerator.Update();
            _playerBrain.Update();
        }
        
        private void FixedUpdate()
        {
            if(isPaused)
                return;
            
            _playerBrain.FixedUpdate();
        }
    }
}