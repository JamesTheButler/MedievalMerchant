using Common.Utility;
using Features.Audio.Music;
using Features.Goods.Config;
using Features.Levels.GameModifiers.Events.Data;
using Features.Player.Caravan.Config;
using Features.Player.Retinue.Config;
using Features.Ticking.Config;
using Features.Towns.Config;
using Features.Towns.Development.Config;
using Features.Towns.Missions.Data;
using Features.Towns.Production.Config;
using Features.Towns.Reputation.Data;
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
        public PriceModifierConfig PriceModifierConfig { get; private set; }

        [field: SerializeField, Required]
        public CaravanConfig CaravanConfig { get; private set; }

        [field: SerializeField, Required]
        public CompanionConfig CompanionConfig { get; private set; }

        [field: SerializeField, Required]
        public EventConfig EventConfig { get; private set; }

        [field: SerializeField, Required]
        public GoodConfig GoodConfig { get; private set; }

        [field: SerializeField, Required]
        public MissionConfig MissionConfig { get; private set; }

        [field: SerializeField, Required]
        public MusicConfig MusicConfig { get; private set; }

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