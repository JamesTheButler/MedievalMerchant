using Infrastructure;
using UnityEngine;

namespace Features.Levels
{
    public sealed class LevelBootstrapper : MonoBehaviour
    {
        private void Start()
        {
            GameplayContext.Initialize();
        }
    }
}