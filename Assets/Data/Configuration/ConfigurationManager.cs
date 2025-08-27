using Data.Setup;
using UnityEngine;

namespace Data.Configuration
{
    public sealed class ConfigurationManager : MonoBehaviour
    {
        public static ConfigurationManager Instance;

        [field: SerializeField]
        public GoodsConfig GoodsConfig { get; private set; }
        
        [field: SerializeField]
        public MarketStateConfig MarketStateConfig { get; private set; }

        [field: SerializeField]
        public GrowthTrendConfig GrowthTrendConfig { get; private set; }
        
        [field: SerializeField]
        public DevelopmentConfig DevelopmentConfig { get; private set; }
        
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