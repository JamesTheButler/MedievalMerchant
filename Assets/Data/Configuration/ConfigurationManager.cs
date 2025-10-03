using Data.Player.Retinue;
using Data.Player.Retinue.Config;
using Data.Towns.Upgrades;
using NaughtyAttributes;
using UI.InventoryUI;
using UnityEngine;

namespace Data.Configuration
{
    public sealed class ConfigurationManager : MonoBehaviour
    {
        public static ConfigurationManager Instance;

        [field: SerializeField, Required]
        public GoodsConfig GoodsConfig { get; private set; }

        [field: SerializeField, Required]
        public AvailabilityConfig AvailabilityConfig { get; private set; }

        [field: SerializeField, Required]
        public TownDevelopmentConfig TownDevelopmentConfig { get; private set; }

        [field: SerializeField, Required]
        public PlayerConfig PlayerConfig { get; private set; }

        [field: SerializeField, Required]
        public Colors Colors { get; private set; }

        [field: SerializeField, Required]
        public RecipeConfig RecipeConfig { get; private set; }

        [field: SerializeField, Required]
        public TierIconConfig TierIconConfig { get; private set; }

        [field: SerializeField, Required]
        public ProducerConfig ProducerConfig { get; private set; }

        [field: SerializeField, Required]
        public TownConfig TownConfig { get; private set; }

        [field: SerializeField, Required]
        public ConditionConfig ConditionConfig { get; private set; }
        
        [field: SerializeField, Required]
        public UpgradeProgressionConfig UpgradeProgressionConfig { get; private set; }

        [field: SerializeField, Required]
        public CompanionConfig CompanionConfig { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}