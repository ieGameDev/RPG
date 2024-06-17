using Assets.Scripts.StaticData;
using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class EnemySpawnerData
    {
        public string Id;
        public EnemyTypeId EnemyTypeId;
        public Vector3 Position;

        public EnemySpawnerData(string id, EnemyTypeId enemyTypeId, Vector3 position)
        {
            Id = id;
            EnemyTypeId = enemyTypeId;
            Position = position;
        }
    }
}