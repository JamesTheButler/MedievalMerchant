using Common.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [CreateAssetMenu(
        fileName = nameof(MovementSpeedEffectData),
        menuName = AssetMenu.EffectsFolder + nameof(MovementSpeedEffectData))]
    public sealed class MovementSpeedEffectData : EffectData
    {
        [field: SerializeField, Range(-1f, 2f)]
        public float SpeedBoostPercent { get; private set; }
    }
}