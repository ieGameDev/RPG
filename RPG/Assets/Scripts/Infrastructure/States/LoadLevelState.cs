using Assets.Scripts.CameraLogic;
using Assets.Scripts.Data;
using Assets.Scripts.Enemy.EnemyLoot;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using Assets.Scripts.Logic;
using Assets.Scripts.Player;
using Assets.Scripts.StaticData;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Infrastructure.States
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticDataService;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.CleanUp();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            LevelStaticData levelData = InitStaticData();
            InitSpawners(levelData);
            InitLootPieces();
            GameObject player = InitPlayer(levelData);
            InitHud(player);
            CameraFollow(player);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private LevelStaticData InitStaticData() => 
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        private void InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
            {
                _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyTypeId);
            }
        }

        private void InitLootPieces()
        {
            foreach (string key in _progressService.Progress.WorldData.LootData.LootPiecesOnScene.Dictionary.Keys)
            {
                LootPiece lootPiece = _gameFactory.CreateLoot();
                lootPiece.GetComponent<UniqueId>().Id = key;
            }
        }

        private GameObject InitPlayer(LevelStaticData levelData) =>
            _gameFactory.CreatePlayer(levelData.InitialPlayerPosition);

        private void InitHud(GameObject player) =>
            _gameFactory.CreateHud()
                .GetComponentInChildren<ActorUI>()
                .Construct(player.GetComponent<PlayerHealth>());

        private void CameraFollow(GameObject player) =>
            Camera.main.GetComponent<CameraFollow>().Follow(player);
    }
}
