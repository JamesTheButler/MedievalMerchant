using Features.Goods.Config;
using Features.Levels.Config;
using Features.Player.Caravan.Config;
using Features.Player.Retinue.Config;
using Features.Towns.Config;
using Features.Towns.Development.Config;
using Features.Towns.Production.Config;
using Features.Towns.Reputation;
using Features.Towns.Reputation.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(
        fileName = nameof(Configurations),
        menuName = AssetMenu.ConfigDataFolder + nameof(Configurations),
        order = 0)]
    public sealed class Configurations : ScriptableObject
    {
        [field: SerializeField, Required]
        public AvailabilityConfig AvailabilityConfig { get; private set; }

        [field: SerializeField, Required]
        public CaravanConfig CaravanConfig { get; private set; }

        [field: SerializeField, Required]
        public CompanionConfig CompanionConfig { get; private set; }

        [field: SerializeField, Required]
        public ConditionConfig ConditionConfig { get; private set; }

        [field: SerializeField, Required]
        public GoodsConfig GoodsConfig { get; private set; }

        [field: SerializeField, Required]
        public ProducerConfig ProducerConfig { get; private set; }

        [field: SerializeField, Required]
        public ReputationConfig ReputationConfig { get; private set; }
        
        [field: SerializeField, Required]
        public TickConfig TickConfig { get; private set; }

        [field: SerializeField, Required]
        public TownConfig TownConfig { get; private set; }

        [field: SerializeField, Required]
        public TownDevelopmentConfig TownDevelopmentConfig { get; private set; }
    }
}