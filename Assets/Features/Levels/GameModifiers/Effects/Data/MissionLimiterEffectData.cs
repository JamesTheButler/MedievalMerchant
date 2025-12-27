using Common.Types;
using Common.Utility;
using Features.Goods.Selector;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [CreateAssetMenu(
        fileName = nameof(MissionLimiterEffectData),
        menuName = AssetMenu.EffectsFolder + nameof(MissionLimiterEffectData))]
    public sealed class MissionLimiterEffectData : EffectData
    {
        [field: SerializeField, InfoBox("Regions that the town CANNOT have to be affected.")]
        public Regions UnaffectedRegions { get; private set; }

        [field: SerializeField]
        public GoodSelectorData GoodSelector { get; private set; }

        [SerializeField]
        private string manualDescription;

        public override string Description => manualDescription;
    }
}