using Data.Goods.Recipes.Config;
using Data.Player.Caravan.Config;
using Data.Player.Retinue.Config;
using Data.Towns.Development.Config;
using Data.Towns.Development.UI.DevelopmentGauge;
using Data.Towns.Production.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Configuration
{
    public sealed class ConfigurationManager : MonoBehaviour
    {
        public static ConfigurationManager Instance;

        [field: SerializeField, Required]
        public AvailabilityConfig AvailabilityConfig { get; private set; }

        [field: SerializeField, Required]
        public CaravanConfig CaravanConfig { get; private set; }

        [field: SerializeField, Required]
        public Colors Colors { get; private set; }

        [field: SerializeField, Required]
        public CompanionConfig CompanionConfig { get; private set; }

        [field: SerializeField, Required]
        public ConditionConfig ConditionConfig { get; private set; }

        [field: SerializeField, Required]
        public GoodsConfig GoodsConfig { get; private set; }

        [field: SerializeField, Required]
        public DevelopmentMilestoneAssets DevelopmentMilestoneAssets { get; private set; }

        [field: SerializeField, Required]
        public RecipeConfig RecipeConfig { get; private set; }

        [field: SerializeField, Required]
        public ProducerConfig ProducerConfig { get; private set; }

        [field: SerializeField, Required]
        public TierIconConfig TierIconConfig { get; private set; }

        [field: SerializeField, Required]
        public TownConfig TownConfig { get; private set; }

        [field: SerializeField, Required]
        public TownDevelopmentConfig TownDevelopmentConfig { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            RecipeConfig.Initialize();
        }
    }
}