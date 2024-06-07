using Assets.Scripts.Data;
using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public EnemyTypeId EnemyTypeId;

        private string _id;
        private bool _slane;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _factory = AllServices.Container.Single<IGameFactory>();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
                _slane = true;
            else
                Spawn();
        }

        private void Spawn()
        {
            GameObject enemy = _factory.CreateEnemy(EnemyTypeId, transform);
            _enemyDeath = enemy.GetComponent<EnemyDeath>();
            _enemyDeath.DeathHappened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.DeathHappened -= Slay;

            _slane = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slane)
                progress.KillData.ClearedSpawners.Add(_id);
        }
    }
}
