using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "enemyData", menuName = "StaticData/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;

        [Range(1,200)]
        public float Hp;

        [Range(1,30)]
        public float Damage;

        [Range(1f, 10)]
        public float MoveSpeed;

        [Range(0.5f,1)]
        public float EffectiveDistance;

        [Range(0.5f, 1)]
        public float Cleavage;

        public GameObject Prefab;
    }
}
