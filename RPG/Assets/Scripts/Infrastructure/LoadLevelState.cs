using Assets.Scripts.CameraLogic;
using Assets.Scripts.Logic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class LoadLevelState : IPayLoadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private const string PlayerPath = "Player/Prefab/FemaleCharacter";
        private const string HudPath = "Hud/Hud";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _curtain.Hide();

        private void OnLoaded()
        {
            GameObject initialPoint = GameObject.FindWithTag(InitialPointTag);
            GameObject player = Instantiate(PlayerPath, initialPoint.transform.position);

            Instantiate(HudPath);

            CameraFollow(player);

            _stateMachine.Enter<GameLoopState>();
        }

        private static GameObject Instantiate(string path)
        {
            var playerPrefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(playerPrefab);
        }

        private static GameObject Instantiate(string path, Vector3 point)
        {
            var playerPrefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(playerPrefab, point, Quaternion.identity);
        }

        private void CameraFollow(GameObject player) =>
            Camera.main.GetComponent<CameraFollow>().Follow(player);
    }
}
