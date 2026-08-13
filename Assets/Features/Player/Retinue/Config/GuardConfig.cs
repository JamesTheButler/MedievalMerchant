using Common.Utility;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [CreateAssetMenu(
        fileName = nameof(GuardConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(GuardConfig))]
    public sealed class GuardConfig : ScriptableObject
    {
        [field: SerializeField]
        public float UpkeepPerGuardPerDay { get; private set; } = 0.3f;

        [field: SerializeField]
        public float HitFactorMin { get; private set; } = 0.85f;

        [field: SerializeField]
        public float HitFactorMax { get; private set; } = 1.15f;
    }
}
