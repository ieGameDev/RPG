using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Enemy.EnemyLoot
{
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        private const float DelayBeforeDestroying = 1.5f;

        [SerializeField] private GameObject _soul;
        [SerializeField] private GameObject _pickupFXPrefab;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickupPopup;

        private Loot _loot;
        private WorldData _worldData;

        private string _id;

        private bool _picked;
        private bool _loadedFromProgress;

        public void Construct(WorldData worldData) =>
            _worldData = worldData;

        private void Start()
        {
            if (!_loadedFromProgress)
                _id = GetComponent<UniqueId>().Id;
        }

        public void Initialize(Loot loot) =>
            _loot = loot;

        public void LoadProgress(PlayerProgress progress)
        {
            _id = GetComponent<UniqueId>().Id;

            LootPieceData data = progress.WorldData.LootData.LootPiecesOnScene.Dictionary[_id];
            Initialize(data.Loot);
            transform.position = data.Position.AsUnityVector();

            _loadedFromProgress = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_picked)
                return;

            LootPieceDataDictionary lootPiecesOnScene = progress.WorldData.LootData.LootPiecesOnScene;

            if (!lootPiecesOnScene.Dictionary.ContainsKey(_id))
                lootPiecesOnScene.Dictionary
                  .Add(_id, new LootPieceData(transform.position.AsVectorData(), _loot));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_picked)
            {
                _picked = true;
                Pickup();
            }
        }

        private void Pickup()
        {
            UpdateWorldData();
            HideSoul();
            PlayPickupFx();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData()
        {
            UpdateCollectedLootAmount();
            RemoveLootPieceFromSavedPieces();
        }

        private void UpdateCollectedLootAmount() => 
            _worldData.LootData.Collect(_loot);

        private void RemoveLootPieceFromSavedPieces()
        {
            LootPieceDataDictionary savedLootPieces = _worldData.LootData.LootPiecesOnScene;

            if (savedLootPieces.Dictionary.ContainsKey(_id))
                savedLootPieces.Dictionary.Remove(_id);
        }

        private void HideSoul() =>
            _soul.SetActive(false);

        private void PlayPickupFx() =>
            Instantiate(_pickupFXPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        private void ShowText()
        {
            _lootText.text = $"{_loot.Value}";
            _pickupPopup.SetActive(true);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(DelayBeforeDestroying);
            Destroy(gameObject);
        }
    }
}
