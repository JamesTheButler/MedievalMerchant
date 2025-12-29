using Common.Infrastructure;
using UnityEngine;

namespace Features.Towns.UI
{
    public abstract class TownUISection : MonoBehaviour, IInitializable
    {
        public abstract void Initialize();
        public abstract void CleanUp();

        public abstract void Bind(Town town);
        public abstract void Unbind(Town town);
    }
}