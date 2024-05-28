using Assets.Scripts.Logic;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Infrastructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, curtain),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad>
        {
            TState state = ChangeState<TState>();
            state.Enter(payLoad);
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }
    }
}
