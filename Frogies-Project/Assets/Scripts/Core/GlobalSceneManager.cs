using System.Collections.Generic;
using System.Linq;
using Animation;
using Core.Entities;
using Core.Entities.Data;
using Core.Entities.Enemies;
using Fighting;
using Items;
using Items.Behaviour;
using Items.Rarity;
using Items.Scriptable;
using Items.Storage;
using JetBrains.Annotations;
using Movement;
using Unity.VisualScripting;
using UnityEngine;

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

        [SerializeField] private PlayerData playerData;
        [SerializeField] private EnemyData enemyData;
        public PlayerInputActions Input { get; private set; }
        [CanBeNull] public Camera GlobalCamera { get; set; }

        public Transform PlayerTransform => playerData.DirectionalMover.transform;

        public BasePrefabsStorage PrefabsStorage => prefabsStorage;

        private ItemSystem _sceneItemStorage;
        private DropGenerator _dropGenerator;

        private bool _isPaused = false;
        
        private HashSet<BasicEntity> _entities;

        private void Awake()
        {
            Debug.Assert(Instance == null);
            Instance = this;

            GlobalCamera = camera;
            Input = new PlayerInputActions();
            Input.Enable();

            _entities = new HashSet<BasicEntity>();
            var player = InitializePlayer(playerData);
            _entities.Add(player);
            _entities.Add(InitializeEnemy(enemyData));

            InitializeItemFactory(player);

            InitializeDropGenerator();
        }

        private void InitializeItemFactory(BasicEntity player)
        {
            ItemFactory factory = new ItemFactory(player.Brain.StatsController);
            _sceneItemStorage = new ItemSystem(
                PrefabsStorage.SceneItemPrefab.GetComponent<SceneItem>(),
                itemRarityDescriptor.RarityDescriptor.Cast<IItemRarityColor>().ToArray(),
                factory);
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
            return player;
        }

        private BasicEntity InitializeEnemy(EnemyData entityData)
        {
            var enemy = new BasicEnemy(entityData);
            return enemy;
        }

        private void InitializeDropGenerator()
        {
            var descriptors = itemsStorage.ItemScriptables.Select(scriptable => scriptable.ItemDescriptor).ToList();
            _dropGenerator = new DropGenerator(playerData.DirectionalMover, _sceneItemStorage, descriptors);
        }

        private void Update()
        {
            if (_isPaused)
                return;

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
    }
}