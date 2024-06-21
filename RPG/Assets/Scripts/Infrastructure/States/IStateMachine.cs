using Assets.Scripts.Infrastructure.Services;

namespace Assets.Scripts.Infrastructure.States
{
    public interface IStateMachine : IService
    {
        void Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad>;
        void Enter<TState>() where TState : class, IState;
    }
}