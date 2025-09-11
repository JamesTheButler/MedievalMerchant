using Data.Setup;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Configuration
{
    public sealed class ConfigurationManager : MonoBehaviour
    {
        public static ConfigurationManager Instance;

        [field: SerializeField, Required]
        public GoodsConfig GoodsConfig { get; private set; }

        [field: SerializeField, Required]
        public MarketStateConfig MarketStateConfig { get; private set; }

        [field: SerializeField, Required]
        public GrowthTrendConfig GrowthTrendConfig { get; private set; }

        [field: SerializeField, Required]
        public DevelopmentConfig DevelopmentConfig { get; private set; }

        [field: SerializeField, Required]
        public PlayerUpgradeConfig PlayerUpgradeConfig { get; private set; }

        [field: SerializeField, Required]
        public Colors Colors { get; private set; }

        [field: SerializeField, Required]
        public RecipeConfig RecipeConfig { get; private set; }
        
        [field: SerializeField, Required]
        public TierIconConfig TierIconConfig { get; private set; }

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