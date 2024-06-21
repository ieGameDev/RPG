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

        GameObject CreatePlayer(Vector3 initialPoint);
        GameObject CreateHud();
        Task<GameObject> CreateEnemyAsync(EnemyTypeId enemyTypeId, Transform parent);
        LootPiece CreateLoot();
        void CreateSpawner(Vector3 position, string spawnerId, EnemyTypeId enemyTypeId);

        void CleanUp();
    }
}
