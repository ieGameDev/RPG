using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string enemyDataPath = "StaticData/Enemy";
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;

        public void LoadEnemies()
        {
            _enemies = Resources
                .LoadAll<EnemyStaticData>(enemyDataPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);
        }

        public EnemyStaticData ForEnemy(EnemyTypeId enemyTypeId) =>
            _enemies.TryGetValue(enemyTypeId, out EnemyStaticData staticData) ? staticData : null;
    }
}
