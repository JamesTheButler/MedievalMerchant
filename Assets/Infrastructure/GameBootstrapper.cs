using UnityEngine;

namespace Infrastructure
{
    public sealed class GameBootstrapper : MonoBehaviour
    {
        private void Start()
        {
            GlobalContext.Services.Initialize();
            GlobalContext.ProgressModel.Initialize();
        }
    }
}