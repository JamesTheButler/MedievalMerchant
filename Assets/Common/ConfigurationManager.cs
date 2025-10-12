using Common.Config;
using Features.Goods.Config;
using Features.Levels.Config;
using Features.Player.Caravan.Config;
using Features.Player.Retinue.Config;
using Features.Towns.Config;
using Features.Towns.Development.Config;
using Features.Towns.Development.UI.DevelopmentGauge;
using Features.Towns.Production.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Common
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