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

        public GameObject CreatePlayer(Vector3 initialPoint)
        {
            PlayerStaticData playerData = _staticData.PlayerData();

            PlayerGameObject = Object.Instantiate(playerData.Prefab, initialPoint, Quaternion.identity);

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

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);

            return hud;
        }

        public GameObject CreateEnemy(EnemyTypeId enemyTypeId, Transform parent)
        {
            EnemyStaticData enemyData = _staticData.ForEnemy(enemyTypeId);
            GameObject enemy = Object.Instantiate(enemyData.Prefab, parent.position, Quaternion.identity, parent);

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

        public LootPiece CreateLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetPath.LootPath)
                .GetComponent<LootPiece>();

            lootPiece.Construct(_progressService.Progress.WorldData);

            return lootPiece;
        }

        public void CreateSpawner(Vector3 position, string spawnerId, EnemyTypeId enemyTypeId)
        {
            EnemySpawnPoint spawner = InstantiateRegistered(AssetPath.Spawner, position).GetComponent<EnemySpawnPoint>();

            spawner.Construct(this);
            spawner.Id = spawnerId;
            spawner.EnemyTypeId = enemyTypeId;
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 position)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
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
