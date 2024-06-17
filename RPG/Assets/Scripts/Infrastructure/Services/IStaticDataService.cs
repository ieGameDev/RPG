using Assets.Scripts.StaticData;

namespace Assets.Scripts.Infrastructure.Services
{
    public interface IStaticDataService : IService
    {
        EnemyStaticData ForEnemy(EnemyTypeId enemyTypeId);
        PlayerStaticData PlayerData();
        void LoadEnemies();
        void LoadPlayer();
        LevelStaticData ForLevel(string sceneKey);
    }
}