using Common;
using Common.Types;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(fileName = nameof(GoodResourceData), menuName = AssetMenu.ConfigDataFolder + nameof(GoodResourceData))]
    public sealed class GoodResourceData : ScriptableObject
    {
        [field: SerializeField]
        public string GoodName { get; private set; }

        [field: SerializeField]
        public Tier Tier { get; private set; }

        [field: SerializeField, Required, ShowAssetPreview]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public Regions Regions { get; private set; }
    }
}