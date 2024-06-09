using UnityEngine;

namespace Assets.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "playerData", menuName = "StaticData/Player")]
    public class PlayerStaticData : ScriptableObject
    {
        [Range(1, 200)]
        public float Hp;

        [Range(1, 30)]
        public float Damage;

        [Range(1f, 10)]
        public float MoveSpeed;

        [Range(0.5f, 1)]
        public float DamageRadius;

        public GameObject Prefab;
    }
}
