using Common.Types;
using Common.Utility;
using Features.Goods.Selector;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [CreateAssetMenu(
        fileName = nameof(MissionLimiterEffectData),
        menuName = AssetMenu.EffectsFolder + nameof(MissionLimiterEffectData))]
    public sealed class MissionLimiterEffectData : EffectData
    {
        [field: SerializeField]
        public Regions AffectedRegions { get; private set; }

        [field: SerializeField]
        public GoodSelectorData GoodSelector { get; private set; }
    }
}