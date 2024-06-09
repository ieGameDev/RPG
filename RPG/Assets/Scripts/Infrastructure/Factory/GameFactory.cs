using Assets.Scripts.Data;
using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.AssetManagement;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
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
        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        private GameObject PlayerGameObject { get; set; }

        public GameFactory(IAssets assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
        }

        public GameObject CreatePlayer(GameObject initialPoint)
        {
            PlayerStaticData playerData = _staticData.PlayerData();

            PlayerGameObject = Object.Instantiate(playerData.Prefab, initialPoint.transform.position, Quaternion.identity);
            //PlayerGameObject = InstantiateRegistered(AssetPath.PlayerPath, initialPoint.transform.position);

            var playerMove = PlayerGameObject.GetComponent<PlayerMove>();
            var playerHealth = PlayerGameObject.GetComponent<PlayerHealth>();
            var playerState = new PlayerState { MaxHP = playerData.Hp, CurrentHP = playerData.Hp };
            var playerAttack = PlayerGameObject.GetComponent<PlayerAttack>();

            playerMove.MovementSpeed = playerData.MoveSpeed;

            playerHealth.Initialize(playerState);
            IHealth health = PlayerGameObject.GetComponent<IHealth>();
            health.CurrentHealth = playerData.Hp;
            health.MaxHealth = playerData.Hp;

            PlayerProgress playerProgress = new PlayerProgress("initialLevel");
            playerProgress.PlayerStats.Damage = playerData.Damage;
            playerProgress.PlayerStats.DamageRadius = playerData.DamageRadius;

            playerAttack.LoadProgress(playerProgress);
            RegisterProgressWatchers(PlayerGameObject);

            return PlayerGameObject;
        }

        public GameObject CreateHud() =>
            InstantiateRegistered(AssetPath.HudPath);

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

            Attack attack = enemy.GetComponent<Attack>();
            attack.Construct(PlayerGameObject.transform);
            attack.Damage = enemyData.Damage;
            attack.Cleavage = enemyData.Cleavage;
            attack.EffectiveDistance = enemyData.EffectiveDistance;

            return enemy;
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

        public void Register(ISavedProgressReader progressReader)
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
