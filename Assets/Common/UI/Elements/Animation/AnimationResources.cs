using Common.Utility;
using UnityEngine;

namespace Common.UI.Elements.Animation
{
    [CreateAssetMenu(
        fileName = nameof(AnimationResources),
        menuName = AssetMenu.ResourceFolder + nameof(AnimationResources))]
    public sealed class AnimationResources : ScriptableObject
    {
        [field: SerializeField, Min(0f)]
        public float PanelSlideInDurationSeconds { get; private set; } = .5f;
    }
}