using Common.Types;
using Common.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Goods.Config
{
    [CreateAssetMenu(fileName = nameof(GoodResourceData),
        menuName = AssetMenu.ConfigDataFolder + nameof(GoodResourceData))]
    public sealed class GoodResourceData : ScriptableObject
    {
        [SerializeField]
        private LocalizedString goodName, productionBuildingName;

        public string GoodName => goodName.GetLocalizedString();
        public string BuildingName => productionBuildingName.GetLocalizedString();
        
        [field: SerializeField]
        public Tier Tier { get; private set; }

        [field: SerializeField, Required, ShowAssetPreview]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public Regions Regions { get; private set; }
    }
}