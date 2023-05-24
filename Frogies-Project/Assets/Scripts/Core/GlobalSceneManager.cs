using System.Collections.Generic;
using System.Linq;
using Animation;
using Core.InventorySystem;
using Core.Entities;
using Core.Entities.Data;
using Core.Entities.Enemies;
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
using StorySystem;
using StorySystem.Behaviour;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using WaveSystem;

namespace Core
{
    public class GlobalSceneManager : MonoBehaviour
    {
        public static GlobalSceneManager Instance { get; private set; }
        public static PlayerInputActions InputInstance => Instance.Input;

        [SerializeField] private new Camera camera;

        [SerializeField] private ItemsStorage itemsStorage;
        [SerializeField] private BasePrefabsStorage prefabsStorage;
        [SerializeField] private ItemRarityDescriptorStorage itemRarityDescriptor;
        [SerializeField] private WaveStorage waveStorage;
        
        [SerializeField] private PotionSystem.PotionSystem potionSystem;
        [SerializeField] private Inventory inventory;
        [SerializeField] private DayTimer dayTimer;

        [SerializeField] private PlayerData playerData;
        [SerializeField] private WaveData waveData;
        [SerializeField] private GameObject testEnemy;
        
        [Header("Story")]
        [SerializeField] private StoryTriggerManager storyTriggerManager;
        [SerializeField] private PlayerActor playerActor;
        [Space(10)]

        [SerializeField] private GameObject DeathPanel;

        private WaveController _waveController;

        public PlayerInputActions Input { get; private set; }
        
        public PixelPerfectCamera GlobalCamera { get; private set; }
        
        public Transform PlayerTransform => playerData.DirectionalMover.transform;

        public BasePrefabsStorage PrefabsStorage => prefabsStorage;
        public PlayerData PlayerData => playerData;

        public StoryDirector StoryDirector => _storyDirector;

        private ItemSystem _sceneItemStorage;
        private DropGenerator _dropGenerator;
        private StoryDirector _storyDirector;

        private bool _isPaused = false;

        private HashSet<BasicEntity> _entities;

        private void Awake()
        {
            Debug.Assert(Instance == null);
            Instance = this;

            GlobalCamera = camera.GetComponent<PixelPerfectCamera>();
            Input = new PlayerInputActions();
            Input.Enable();

            DeathPanel.SetActive(false);
            
            _entities = new HashSet<BasicEntity>();
            var player = InitializePlayer(playerData);
            _entities.Add(player);
            _entities.Add(InitializeEnemy(testEnemy, out _));

            var descriptors = itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            
            InitializeItemFactory(player);
            InitializePotionSystem(descriptors, player);
            InitializeDropGenerator(descriptors);
            InitializeWaveSystem();
            InitializeStoryDirector();
            InitializeDayTimer();
        }

        private void InitializeDayTimer()
        {
            dayTimer.OnDayEnd += potionSystem.OpenPotionMenu;
            _waveController.OnWaveCleared += dayTimer.ResetTimer;
            potionSystem.OnActive += dayTimer.ClearTimer;
        }

         private void InitializeItemFactory(BasicEntity player)
         {
             ItemFactory factory = new ItemFactory(player.Brain.StatsController);
             _sceneItemStorage = new ItemSystem(
                 PrefabsStorage.SceneItemPrefab.GetComponent<SceneItem>(), 
                 itemRarityDescriptor.RarityDescriptor.Cast<IItemRarityColor>().ToArray(), 
                 factory, inventory);
         }

        private BasicEntity InitializePlayer(PlayerData entityData)
        {
            entityData.DirectionalMover.GameObject();
            PlayerMoveInputReader moveInputReader = new PlayerMoveInputReader(Input);
            PlayerFightInputReader fightInputReader = new PlayerFightInputReader(Input, entityData.AttacksData);
            PlayerAnimationController playerAnimation =
                new PlayerAnimationController(entityData.AnimationStateManager, entityData.SpriteFlipper);
            var statsStorage = prefabsStorage.StatsStorage;
            var entityBrain = new EntityBrain(entityData.MovementData, entityData.AttacksData, moveInputReader,
                fightInputReader, entityData.DirectionalMover,
                playerAnimation, statsStorage, entityData.AttackColliders);
            entityData.HealthBar.Setup(entityBrain.StatsController);
            entityData.EnduranceControlBar.Setup(entityBrain.StatsController);
            var player = new Entities.Player.Player();
            player.Initialize(entityBrain);

            entityData.DamageReceiver.Initialize(entityBrain.HealthSystem.TakeDamage);

            entityBrain.HealthSystem.OnDead += (_, _) => DeathPanel.SetActive(true); 
            return player;
        }

        public BasicEntity InitializeEnemy(GameObject enemyPrefab, out GameObject enemyGameObject)
        {
            enemyGameObject = Instantiate(enemyPrefab);
            var enemyData = enemyGameObject.GetComponent<EnemyDataComponent>();
            enemyData.Data.Player = playerData.DirectionalMover.transform;
            var basicEnemy = new BasicEnemy(enemyData.Data);
            _entities.Add(basicEnemy);
            return basicEnemy;
        }

         private void InitializePotionSystem(List<ItemDescriptor> itemDescriptors, BasicEntity player)
         {
             var depowerPotions = itemDescriptors.Where(descriptor => descriptor.ItemId == ItemId.DepowerPotion)
                 .Select(descriptor => new Potion(descriptor as StatChangingItemDescriptor, player.Brain.StatsController)).ToList();
             potionSystem.Setup(depowerPotions);
             potionSystem.OnActive += () => _isPaused = true;
             potionSystem.OnOptionSelected += _ => _isPaused = false;
         }
        
        private void InitializeDropGenerator(List<ItemDescriptor> itemDescriptors)
        {
            _dropGenerator = new DropGenerator(playerData.DirectionalMover, _sceneItemStorage, itemDescriptors);
        }
        
        private void InitializeWaveSystem()
        {
            var waves = waveStorage.Waves.Select(wave => wave.GetCopy()).ToDictionary(wave => wave);
            _waveController = new WaveController(waves, waveData.Spawners, waveData.Enemies);
            waveData.WaveBar.Setup(_waveController);
            potionSystem.OnOptionSelected += _waveController.OnPotionPicked;
        }
        
        private void InitializeStoryDirector()
        {
            playerActor.Init();
            
            _storyDirector = new StoryDirector();
            storyTriggerManager.InitTriggers(playerActor, _storyDirector);
        }

        private void Update()
        {
            if (_isPaused)
                return;
            
            dayTimer.UpdateTimer();
            
            // TODO: remove
            if (UnityEngine.Input.GetKeyUp(KeyCode.P)) // for testing purpose only
            {
                potionSystem.OpenPotionMenu();
            }
            
            _waveController.EnemyChecker();
            // TODO: remove
            if (UnityEngine.Input.GetKeyDown(KeyCode.K))// for testing purpose only
            {
                _waveController.OnPotionPicked(0);
            }

            _dropGenerator.Update();
            
            foreach (var entity in _entities)
            {
                entity.Update();
            }
        }

        private void FixedUpdate()
        {
            if (_isPaused)
                return;

            foreach (var entity in _entities)
            {
                entity.FixedUpdate();
            }
        }

        public void RestartLevel()
        {
            //TODO: proper level restart
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index);
        }
    }
}