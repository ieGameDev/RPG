using Assets.Scripts.StaticData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Services
{
    public class StaticDataService : IStaticDataService
    {
        private const string enemyDataPath = "StaticData/Enemy";
        private const string playerDataPath = "StaticData/Player/PlayerData";
        private const string levelDataPath = "StaticData/Levels";

        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<string, LevelStaticData> _levels;

        private PlayerStaticData _player;

        public void LoadEnemies()
        {
            _enemies = Resources
                .LoadAll<EnemyStaticData>(enemyDataPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);

            _levels = Resources
                .LoadAll<LevelStaticData>(levelDataPath)
                .ToDictionary(x => x.LevelKey, x => x);
        }

        public void LoadPlayer() =>
            _player = Resources.Load<PlayerStaticData>(playerDataPath);

        public EnemyStaticData ForEnemy(EnemyTypeId enemyTypeId) =>
            _enemies.TryGetValue(enemyTypeId, out EnemyStaticData staticData) ? staticData : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;

        public PlayerStaticData PlayerData() =>
            _player;

    }
}
