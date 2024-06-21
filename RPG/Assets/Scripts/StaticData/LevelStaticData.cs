using Assets.Scripts.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Levels")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;

        public List<EnemySpawnerData> EnemySpawners;

        public Vector3 InitialPlayerPosition;
    }
}