using Assets.Scripts.Infrastructure.AssetManagement;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }

        public GameObject CreatePlayer(GameObject initialPoint) =>
            _assets.Instantiate(AssetPath.PlayerPath, initialPoint.transform.position);

        public GameObject CreateHud() =>
            _assets.Instantiate(AssetPath.HudPath);
    }
}
