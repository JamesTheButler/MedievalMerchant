using Common.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Bandits.Data
{
    [CreateAssetMenu(
        fileName = nameof(BanditResources),
        menuName = AssetMenu.ResourceFolder + nameof(BanditResources))]
    public sealed class BanditResources : ScriptableObject
    {
        [field: SerializeField, Required, ShowAssetPreview]
        public Sprite BanditUnitIcon { get; private set; }

        [field: SerializeField, Required, ShowAssetPreview]
        public Sprite BanditCommanderIcon { get; private set; }
    }
}