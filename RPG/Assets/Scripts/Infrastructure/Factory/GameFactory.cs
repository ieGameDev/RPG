using Assets.Scripts.Data;
using Assets.Scripts.Enemy;
using Assets.Scripts.Enemy.EnemyLoot;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.Logic;
using Assets.Scripts.Logic.EnemySpawners;
using Assets.Scripts.Player;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private const string InitialLevel = "Initial";

        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject PlayerGameObject { get; set; }

        public GameFactory(IAssets assets, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService)
        {
            _assets = assets;
            _staticData = staticData;
            _randomService = randomService;
            _progressService = progressService;
        }

        public async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetAddress.Loot);
            await _assets.Load<GameObject>(AssetAddress.Spawner);
        }

        public async Task<GameObject> CreatePlayer(Vector3 initialPoint)
        {
            PlayerStaticData playerData = _staticData.PlayerData();

            PlayerGameObject = await InstantiateRegisteredAsync(AssetAddress.Player, initialPoint);
            //PlayerGameObject = Object.Instantiate(playerData.Prefab, initialPoint, Quaternion.identity);

            PlayerMove playerMove = PlayerGameObject.GetComponent<PlayerMove>();
            PlayerHealth playerHealth = PlayerGameObject.GetComponent<PlayerHealth>();
            PlayerState playerState = new PlayerState { MaxHP = playerData.Hp, CurrentHP = playerData.Hp };
            PlayerAttack playerAttack = PlayerGameObject.GetComponent<PlayerAttack>();

            playerMove.MovementSpeed = playerData.MoveSpeed;

            playerHealth.Initialize(playerState);
            IHealth health = PlayerGameObject.GetComponent<IHealth>();
            health.CurrentHealth = playerData.Hp;
            health.MaxHealth = playerData.Hp;

            PlayerProgress playerProgress = _progressService.Progress;
            playerProgress.PlayerStats.Damage = playerData.Damage;
            playerProgress.PlayerStats.DamageRadius = playerData.DamageRadius;

            playerAttack.LoadProgress(playerProgress);
            RegisterProgressWatchers(PlayerGameObject);

            return PlayerGameObject;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.Hud);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);

            return hud;
        }

        public async Task<GameObject> CreateEnemy(EnemyTypeId enemyTypeId, Transform parent)
        {
            EnemyStaticData enemyData = _staticData.ForEnemy(enemyTypeId);

            GameObject prefab = await _assets.Load<GameObject>(enemyData.PrefabReference);
            GameObject enemy = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            IHealth health = enemy.GetComponent<IHealth>();
            health.CurrentHealth = enemyData.Hp;
            health.MaxHealth = enemyData.Hp;

            enemy.GetComponent<ActorUI>().Construct(health);
            enemy.GetComponent<AgentMoveToPlayer>().Construct(PlayerGameObject.transform);
            enemy.GetComponent<NavMeshAgent>().speed = enemyData.MoveSpeed;

            LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(enemyData.MinLoot, enemyData.MaxLoot);
            lootSpawner.Construct(this, _randomService);

            Attack attack = enemy.GetComponent<Attack>();
            attack.Construct(PlayerGameObject.transform);
            attack.Damage = enemyData.Damage;
            attack.Cleavage = enemyData.Cleavage;
            attack.EffectiveDistance = enemyData.EffectiveDistance;

            return enemy;
        }

        public async Task<LootPiece> CreateLoot()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Loot);
            LootPiece lootPiece = InstantiateRegistered(prefab)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);

            return lootPiece;
        }

        public async Task CreateSpawner(Vector3 position, string spawnerId, EnemyTypeId enemyTypeId)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
            EnemySpawnPoint spawner = InstantiateRegistered(prefab, position)
                .GetComponent<EnemySpawnPoint>();

            spawner.Construct(this);
            spawner.Id = spawnerId;
            spawner.EnemyTypeId = enemyTypeId;
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();

            _assets.CleanUp();
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 position)
        {
            GameObject gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 position)
        {
            GameObject gameObject = await _assets.Instantiate(prefabPath, position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _assets.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }
    }
}
