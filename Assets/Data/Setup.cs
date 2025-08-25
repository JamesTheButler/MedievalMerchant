using UnityEngine;

namespace Data
{
    public class Setup : MonoBehaviour
    {
        public static Setup Instance;

        [field: SerializeField]
        public GoodInfoManager GoodInfoManager { get; private set; }
        
        [field: SerializeField]
        public DevelopmentSetup DevelopmentSetup { get; private set; }
        
        [field: SerializeField]
        public Colors Colors { get; private set; }

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