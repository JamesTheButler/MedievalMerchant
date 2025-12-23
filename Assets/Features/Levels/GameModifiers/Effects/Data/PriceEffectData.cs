using Common.Utility;
using Features.Goods;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [CreateAssetMenu(
        fileName = nameof(PriceEffectData),
        menuName = AssetMenu.EffectsFolder + nameof(PriceEffectData))]
    public sealed class PriceEffectData : EffectData
    {
        [field: SerializeField, Range(-1f, 2f)]
        public float PriceBoostPercent { get; private set; }

        [field: SerializeField]
        public GoodSelectorData GoodSelector { get; private set; }
    }
}