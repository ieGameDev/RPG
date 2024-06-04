using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Infrastructure.Services.PersistentProgress;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject PlayerGameObject { get; }

        event Action PlayerCreated;

        GameObject CreatePlayer(GameObject initialPoint);
        GameObject CreateHud();
        void CleanUp();
        void Register(ISavedProgressReader progressReader);
    }
}
