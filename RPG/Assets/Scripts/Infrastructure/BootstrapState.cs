using Assets.Scripts.Services.Input;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.Load(Initial, EnterLoadLevel);
        }

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadLevelState, string>("Main");

        private void RegisterServices()
        {
            Game.InputService = RegisterInputService();
        }

        public void Exit()
        {
            
        }

        private static IInputService RegisterInputService()
        {
            if (Application.isEditor)
                return new StandalobeInputService();
            else
                return new MobileInputService();
        }
    }
}
