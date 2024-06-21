using Assets.Scripts.Infrastructure.Services;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        public GameObject Instantiate(string path);
        public GameObject Instantiate(string path, Vector3 point);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        void CleanUp();
    }
}