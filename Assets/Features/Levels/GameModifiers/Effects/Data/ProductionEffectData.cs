using Common.Utility;
using Features.Goods;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [CreateAssetMenu(
        fileName = nameof(ProductionEffectData),
        menuName = AssetMenu.EffectsFolder + nameof(ProductionEffectData))]
    public sealed class ProductionEffectData : EffectData
    {
        [field: SerializeField, Range(-1f, 2f)]
        public float SpeedBoostPercent { get; private set; }

        [field: SerializeField]
        public GoodSelectorData Selector { get; private set; }
    }
}