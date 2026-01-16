using UnityEngine;

namespace Common.UI.Elements
{
    public abstract class InitializableSingleton : MonoBehaviour
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized)
                return;

            OnInitialize();
            
            _isInitialized = true;
        }

        protected abstract void OnInitialize();
    }
}