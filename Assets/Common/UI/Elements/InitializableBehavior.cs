using Common.Infrastructure;
using UnityEngine;

namespace Common.UI.Elements
{
    public abstract class InitializableBehavior : MonoBehaviour, IInitializable
    {
        public abstract void Initialize();

        public virtual void CleanUp() { }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}