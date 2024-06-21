using Assets.Scripts.Infrastructure.Services;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        public Task<GameObject> Instantiate(string path);
        public Task<GameObject> Instantiate(string path, Vector3 point);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void CleanUp();
        void Initialize();
    }
}