using Common.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [CreateAssetMenu(
        fileName = nameof(ReputationEffectData),
        menuName = AssetMenu.EffectsFolder + nameof(ReputationEffectData))]
    public sealed class ReputationEffectData : EffectData
    {
        [field: SerializeField, Range(-1f, 2f)]
        public float ReputationBoostPercent { get; private set; }
    }
}