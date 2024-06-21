using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services.Randomizer;
using Assets.Scripts.Logic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy.EnemyLoot
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _enemyDeath;

        private IGameFactory _factory;
        private IRandomService _random;

        private int _lootMin;
        private int _lootMax;

        public void Construct(IGameFactory gameFactory, IRandomService random)
        {
            _factory = gameFactory;
            _random = random;
        }

        private void Start() => 
            _enemyDeath.DeathHappened += SpawnLoot;

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }

        private async void SpawnLoot()
        {
            _enemyDeath.DeathHappened -= SpawnLoot;
            await SpawnAsync();
        }

        private async Task SpawnAsync()
        {
            await Task.Delay(1500);

            LootPiece loot = await _factory.CreateLoot();
            loot.transform.position = transform.position;
            loot.GetComponent<UniqueId>().GenerateId();

            Loot lootItem = GenerateLoot();

            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            Loot loot = new Loot()
            {
                Value = _random.Next(_lootMin, _lootMax)
            };
            return loot;
        }
    }
}
