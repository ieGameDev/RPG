using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string enemyDataPath = "StaticData/Enemy";
        private const string playerDataPath = "StaticData/Player/PlayerData";

        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private PlayerStaticData _player;

        public void LoadEnemies()
        {
            _enemies = Resources
                .LoadAll<EnemyStaticData>(enemyDataPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);
        }

        public void LoadPlayer() => 
            _player = Resources.Load<PlayerStaticData>(playerDataPath);

        public EnemyStaticData ForEnemy(EnemyTypeId enemyTypeId) =>
            _enemies.TryGetValue(enemyTypeId, out EnemyStaticData staticData) ? staticData : null;

        public PlayerStaticData PlayerData() => 
            _player;
    }
}
