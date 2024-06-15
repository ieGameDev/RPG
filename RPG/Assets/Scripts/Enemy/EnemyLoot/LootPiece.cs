using Assets.Scripts.Data;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Enemy.EnemyLoot
{
    public class LootPiece : MonoBehaviour
    {
        [SerializeField] private GameObject _soul;
        [SerializeField] private GameObject _pickupFXPrefab;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private GameObject _pickupPopup;

        private Loot _loot;
        private WorldData _worldData;
        private bool _picked;

        public void Construct(WorldData worldData) =>
            _worldData = worldData;

        public void Initialize(Loot loot) =>
            _loot = loot;

        private void OnTriggerEnter(Collider other) =>
            Picup();

        private void Picup()
        {
            if (_picked)
                return;

            _picked = true;

            UpdateWorldData();
            HideSoul();
            PlayPickupFx();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData() =>
            _worldData.LootData.Collect(_loot);

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
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}
