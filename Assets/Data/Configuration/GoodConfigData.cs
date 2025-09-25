using NaughtyAttributes;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = nameof(GoodConfigData), menuName = AssetMenu.ConfigDataFolder + nameof(GoodConfigData))]
    public sealed class GoodConfigData : ScriptableObject
    {
        [field: SerializeField]
        public string GoodName { get; private set; }

        [field: SerializeField]
        public Tier Tier { get; private set; }

        [field: SerializeField, Required, ShowAssetPreview]
        public Sprite Icon { get; private set; }
    }
}