using Common.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.Development.UI.DevelopmentGauge
{
    [CreateAssetMenu(
        fileName = nameof(DevelopmentMilestoneResources),
        menuName = AssetMenu.ResourceFolder + nameof(DevelopmentMilestoneResources))]
    public sealed class DevelopmentMilestoneResources : ScriptableObject
    {
        [field: SerializeField, Required]
        public Sprite TopComplete { get; private set; }
    }
}