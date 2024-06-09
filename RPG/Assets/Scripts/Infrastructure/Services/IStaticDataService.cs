using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.StaticData
{
    public interface IStaticDataService : IService
    {
        EnemyStaticData ForEnemy(EnemyTypeId enemyTypeId);
        PlayerStaticData PlayerData();
        void LoadEnemies();
        void LoadPlayer();
    }
}