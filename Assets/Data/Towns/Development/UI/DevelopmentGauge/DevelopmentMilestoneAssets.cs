using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Towns.Development.UI.DevelopmentGauge
{
    [CreateAssetMenu(
        fileName = nameof(DevelopmentMilestoneAssets),
        menuName = AssetMenu.ConfigDataFolder + nameof(DevelopmentMilestoneAssets))]
    public sealed class DevelopmentMilestoneAssets : ScriptableObject
    {
        [field: SerializeField, Required]
        public Sprite BaseIncomplete { get; private set; }

        [field: SerializeField, Required]
        public Sprite BaseComplete { get; private set; }

        [field: SerializeField, Required]
        public Sprite TopIncomplete { get; private set; }

        [field: SerializeField, Required]
        public Sprite TopComplete { get; private set; }
    }
}