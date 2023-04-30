using System;
using System.Linq;
using Animation;
using Core.Player;
using Fighting;
using Items;
using Items.Behaviour;
using Items.Enum;
using Items.Rarity;
using Items.Scriptable;
using Items.Storage;
using JetBrains.Annotations;
using Movement;
using StatsSystem;
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

        [SerializeField] private HealthBar playerHealthBar;
        
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
            statsStorage = Resources.Load<StatsStorage>($"Player/{nameof(StatsStorage)}");
            _playerBrain = new PlayerBrain(_movementData, _attacksData, moveInputReader, fightInputReader, player, attacker, playerAnimation, statsStorage);
            playerHealthBar.Setup(_playerBrain.StatsController);

            ItemFactory factory = new ItemFactory();
            _sceneItemStorage = new ItemSystem(
                PrefabsStorage.SceneItemPrefab.GetComponent<SceneItem>(), 
                itemRarityDescriptor.RarityDescriptor.Cast<IItemRarityColor>().ToArray(), 
                factory);
            
            var descriptors = itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            _dropGenerator = new DropGenerator(player, _sceneItemStorage, descriptors);
        }

        private void Update()
        {
            if(isPaused)
                return;
            
            _dropGenerator.Update();
        }
        
        private void FixedUpdate()
        {
            if(isPaused)
                return;
            
            _playerBrain.FixedUpdate();
        }
    }
}