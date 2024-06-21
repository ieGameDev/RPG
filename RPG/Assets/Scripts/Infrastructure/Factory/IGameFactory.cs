using Assets.Scripts.Enemy.EnemyLoot;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        Task<GameObject> CreatePlayer(Vector3 initialPoint);
        Task<GameObject> CreateHud();
        Task<GameObject> CreateEnemy(EnemyTypeId enemyTypeId, Transform parent);
        Task<LootPiece> CreateLoot();
        Task CreateSpawner(Vector3 position, string spawnerId, EnemyTypeId enemyTypeId);

        void CleanUp();
        Task WarmUp();
    }
}
