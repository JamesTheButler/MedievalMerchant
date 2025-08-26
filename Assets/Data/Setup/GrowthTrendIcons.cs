using System.Collections.Generic;
using Data.Configuration;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "GrowthTrendIcons", menuName = AssetMenu.ConfigDataFolder + "GrowthTrendIcons")]
    public sealed class GrowthTrendIcons : ScriptableObject
    {
        [SerializeField]
        private Sprite veryDownIcon;

        [SerializeField]
        private Sprite downIcon;

        [SerializeField]
        private Sprite balancedIcon;

        [SerializeField]
        private Sprite upIcon;

        [SerializeField]
        private Sprite veryUpIcon;

        public IReadOnlyDictionary<GrowthTrend, Sprite> Icons => _icons ??= GenerateDictionary();

        private Dictionary<GrowthTrend, Sprite> _icons;

        private Dictionary<GrowthTrend, Sprite> GenerateDictionary()
        {
            return new Dictionary<GrowthTrend, Sprite>
            {
                { GrowthTrend.VeryDown, veryDownIcon },
                { GrowthTrend.Down, downIcon },
                { GrowthTrend.Balanced, balancedIcon },
                { GrowthTrend.Up, upIcon },
                { GrowthTrend.VeryU, veryUpIcon },
            };
        }
    }
}