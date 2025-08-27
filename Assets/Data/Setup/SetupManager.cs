using UnityEngine;

namespace Data.Setup
{
    public sealed class SetupManager : MonoBehaviour
    {
        public static SetupManager Instance;

        [field: SerializeField]
        public DevelopmentSetup DevelopmentSetup { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}