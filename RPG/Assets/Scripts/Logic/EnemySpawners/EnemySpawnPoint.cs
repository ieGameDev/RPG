using Assets.Scripts.Data;
using Assets.Scripts.Enemy;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using UnityEngine;

namespace Assets.Scripts.Logic.EnemySpawners
{
    public class EnemySpawnPoint : MonoBehaviour, ISavedProgress
    {
        public EnemyTypeId EnemyTypeId;

        private bool _slane;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public string Id { get; set; }

        public void Construct(IGameFactory factory) =>
            _factory = factory;

        private void OnDestroy()
        {
            if (_enemyDeath != null)
                _enemyDeath.DeathHappened -= Slay;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                _slane = true;
            else
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slane)
                progress.KillData.ClearedSpawners.Add(Id);
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
    }
}
