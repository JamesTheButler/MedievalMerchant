using Common.Infrastructure;
using UnityEngine;

namespace Common.UI.Elements
{
    public abstract class InitializableUI : MonoBehaviour, IInitializable
    {
        public abstract void Initialize();

        public virtual void CleanUp() { }
    }
}