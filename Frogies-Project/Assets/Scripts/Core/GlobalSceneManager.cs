using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animation;
using Core.InventorySystem;
using Core.Entities;
using Core.Entities.Data;
using Core.Entities.Player;
using Core.Entities.Spawners;
using Core.ObjectPoolers;
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
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using WaveSystem;

namespace Core
{
    public class GlobalSceneManager : MonoBehaviour
    {
        public static GlobalSceneManager Instance { get; private set; }
        public static PlayerInputActions InputInstance => Instance.PlayerInputActions;

        [SerializeField] private new Camera camera;

        [SerializeField] private ItemsStorage itemsStorage;
        [SerializeField] private BasePrefabsStorage prefabsStorage;
        [SerializeField] private ItemRarityDescriptorStorage itemRarityDescriptor;
        [SerializeField] private WaveStorage waveStorage;

        [SerializeField] private PotionSystem.PotionSystem potionSystem;
        [SerializeField] private Inventory inventory;
        [SerializeField] private DayTimer dayTimer;
        [SerializeField] private Transform playerSpawner;

        [SerializeField] private PlayerData playerData;
        [SerializeField] private WaveData waveData;

        [SerializeField] private Transform[] deathSpawnPoints;
        [SerializeField] private LayerMask deathSpawnPointsLayerMask;
        
        [Header("Story")] [SerializeField] private StoryTriggerManager storyTriggerManager;
        [SerializeField] private PlayerActor playerActor;
        [SerializeField] [CanBeNull] private ActorSpawner _actorSpawner;
        [SerializeField] private ActorSpawnerDataComponent _deathActorSpawnerDataComponent;
        [Space(10)] [SerializeField] [CanBeNull] private GameObject deathPanel;
        [Space(10)] [SerializeField] [CanBeNull] private GameObject winPanel;

        [SerializeField] private GameObject particleEffectsPoller;
        private WaveController _waveController;
        public EnemySpawner EnemySpawner { get; private set; }

        public PlayerInputActions PlayerInputActions { get; private set; }

        public PixelPerfectCamera GlobalCamera { get; private set; }

        public Transform PlayerTransform => playerData.DirectionalMover.transform;

        public BasePrefabsStorage PrefabsStorage => prefabsStorage;
        public PlayerData PlayerData => playerData;
        public StoryDirector StoryDirector => _storyDirector;
        public DropGenerator DropGenerator => _dropGenerator;
        public PotionSystem.PotionSystem PotionSystem => potionSystem;

        private ItemSystem _sceneItemStorage;
        private DropGenerator _dropGenerator;
        private StoryDirector _storyDirector;

        private bool _isPaused = false;
        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (value == _isPaused)
                {
                    return;
                }
                
                Time.timeScale = value ? 0 : 1;
                _isPaused = value;

                if (value)
                {
                    OnPauseStarted?.Invoke();
                }
                else
                {
                    OnPauseFinished?.Invoke();
                }
            }
        }
        public event Action OnPauseStarted;
        public event Action OnPauseFinished;

        public HashSet<BasicEntity> Entities { get; private set; }
        public PlayerBasicEntity PlayerBasicEntity { get; private set; }

        private void Awake()
        {
            Debug.Assert(Instance == null);
            Instance = this;
            
            GlobalCamera = camera.GetComponent<PixelPerfectCamera>();
            InitializeInput();

            if (deathPanel != null) deathPanel.SetActive(false);
                
            ObjectPooler.Instance.AddOrUpdatePooler(new ObjectPooler.Pool()
            {
                Tag = ObjectPoolTags.HIT_BLOOD_PARTICLE_EFFECTS,
                Prefab = prefabsStorage.HitBloodParticlesSystem.gameObject,
                Size = 30,
                Parent = particleEffectsPoller
            });
            
            ObjectPooler.Instance.AddOrUpdatePooler(new ObjectPooler.Pool()
            {
                Tag = ObjectPoolTags.DECAL_BLOOD_PARTICLE_EFFECTS,
                Prefab = prefabsStorage.HitDecalsParticlesSystem.gameObject,
                Size = 30,
                Parent = particleEffectsPoller
            });

            if (deathPanel != null) 
                deathPanel.SetActive(false);

            EnemySpawner = new EnemySpawner(this);
            
            Entities = new HashSet<BasicEntity>();
            var player = InitializePlayer(playerData);
            PlayerBasicEntity = (PlayerBasicEntity)player;
            Entities.Add(player);

            var descriptors = itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();

            InitializeItemFactory(player);
            InitializePotionSystem(descriptors, player);
            InitializeDropGenerator(descriptors);
            InitializeWaveSystem();
            InitializeStoryDirector();
            InitializeDayTimer();
        }

        private void InitializeInput()
        {
            PlayerInputActions = new PlayerInputActions();
            PlayerInputActions.Enable();
            
            PlayerInputActions.Debug.Disable();
            bool isMouseSchemeEnabled = true;
            PlayerInputActions.Debug.DisableMouseScheme.performed += context =>
            {
                PlayerInputActions.bindingMask = isMouseSchemeEnabled
                    ? InputBinding.MaskByGroups(PlayerInputActions.KeyboardScheme.bindingGroup, PlayerInputActions.OnScreenScheme.bindingGroup)
                    : null;
                isMouseSchemeEnabled = !isMouseSchemeEnabled;
            };
        }

        private void InitializeDayTimer()
        {
            if (_actorSpawner != null)
            {
                dayTimer.OnDayEnd += ()=> _actorSpawner.SpawnActor((PlayerTransform.position+new Vector3(1, 0)));
                _actorSpawner.onActorDialogFinished += potionSystem.OpenPotionMenu;
            }
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
            PlayerMoveInputReader moveInputReader = new PlayerMoveInputReader(PlayerInputActions);
            PlayerFightInputReader fightInputReader =
                new PlayerFightInputReader(PlayerInputActions, entityData.AttacksData);
            PlayerAnimationController playerAnimation =
                new PlayerAnimationController(entityData.AnimationStateManager, entityData.SpriteFlipper);
            var statsStorage = prefabsStorage.StatsStorage;
            var entityBrain = new EntityBrain(entityData.MovementData, entityData.AttacksData, moveInputReader,
                fightInputReader, entityData.DirectionalMover,
                playerAnimation, statsStorage, entityData.AttackColliders);
            entityData.HealthBar.Setup(entityBrain.StatsController);
            entityData.EnduranceControlBar.Setup(entityBrain.StatsController);
            var player = new Entities.Player.PlayerBasicEntity();
            player.Initialize(entityBrain);

            entityData.DamageReceiver.Initialize(entityBrain.HealthSystem.TakeDamage);
            entityData.DamageReceiver.Initialize(entityData.DirectionalMover.Knockback);

            entityBrain.HealthSystem.OnDead += OnHealthSystemOnOnDead;
            
            if (entityData.HitVisualisation.IsEnabled)
            {
                var collider = entityData.DirectionalMover.GetComponent<BoxCollider2D>();
                var bounds = new Bounds(collider.offset, collider.size);
            
                DamageVisuals damageVisuals = new DamageVisuals(
                    entityData.DirectionalMover.transform,
                    bounds, 
                    new DamageVisuals.DamageVisualsData()
                    {
                        BloodColor = entityData.HitVisualisation.BloodColor
                    }
                );
                
                entityData.DamageReceiver.Initialize(damageVisuals.TriggerEffect);
                entityBrain.HealthSystem.OnDead += (_, _) => damageVisuals.Return();
            }
            
            return player;
        }

        private void OnHealthSystemOnOnDead(object o, EventArgs eventArgs)
        {
            if (deathPanel != null) 
                deathPanel.SetActive(true);
        }

        private void InitializePotionSystem(List<ItemDescriptor> itemDescriptors, BasicEntity player)
         {
             var depowerPotions = itemDescriptors.Where(descriptor => descriptor.ItemId == ItemId.DepowerPotion)
                 .Select(descriptor => new Potion(descriptor as StatChangingItemDescriptor, player.Brain.StatsController)).ToList();
             potionSystem.Setup(depowerPotions);
             potionSystem.OnActive += () => IsPaused = true;
             potionSystem.OnOptionSelected += (_,_) => IsPaused = false;
         }
         
        private void InitializeDropGenerator(List<ItemDescriptor> itemDescriptors)
        {
            _dropGenerator = new DropGenerator(playerData.DirectionalMover, _sceneItemStorage, itemDescriptors);
        }

        private void InitializeWaveSystem()
        {
            var waves = waveStorage.Waves.Select(wave => wave.GetCopy()).ToDictionary(wave => wave);
            _waveController = new WaveController(waves, waveData.Spawners, waveData.Enemies, EnemySpawner);
            waveData.WaveBar.Setup(_waveController);
            potionSystem.OnOptionSelected += _waveController.OnPotionPicked;
            _waveController.OnLastWaveCleared += PerformEndGameLogic;
        }

        private void PerformEndGameLogic()
        {
            StartCoroutine(DeathWithDelay());
            if (winPanel != null) 
                winPanel.SetActive(true);
        }

        private IEnumerator DeathWithDelay()
        {
            yield return new WaitForSeconds(2f);

            PlayerBasicEntity.Brain.HealthSystem.OnDead -= OnHealthSystemOnOnDead;
            PlayerBasicEntity.Brain.HealthSystem.TakeDamage
                (new DamageInfo(float.MaxValue,
                    new KnockbackInfo(0)));
        }

        private void InitializeStoryDirector()
        {
            playerActor.Init();
            _storyDirector = new StoryDirector();
            _storyDirector.StoryStarted += () => IsPaused = true;
            _storyDirector.StoryFinished += () => IsPaused = false;
            
            if (_actorSpawner != null)
            {
                _actorSpawner.Init(_storyDirector, _deathActorSpawnerDataComponent);
                _storyDirector.StoryFinished += _actorSpawner.HideActor;
            }

            storyTriggerManager.InitTriggers(playerActor, _storyDirector);
        }

        private void Update()
        {
            if (IsPaused)
                return;

            dayTimer.UpdateTimer();

            _waveController.EnemyChecker();

            _deathActorSpawnerDataComponent.Data.UpdateStartNodeNumber();
            foreach (var entity in Entities)
            {
                entity.Update();
            }
        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;

            foreach (var entity in Entities)
            {
                entity.FixedUpdate();
            }
        }

        public void RestartLevel()
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index);
        }

        public void RespawnPlayer()
        {
            PlayerTransform.position = playerSpawner.position;
        }
    }
}