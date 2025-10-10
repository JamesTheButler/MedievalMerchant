using System;
using AYellowpaper.SerializedCollections;
using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Player.Caravan.Config
{
    [CreateAssetMenu(
        fileName = nameof(CaravanConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(CaravanConfig))]
    public sealed class CaravanConfig : ScriptableObject
    {
        public const int MaxCartCount = 4;
        public const int MaxLevel = 4;
        public const float BaseUpkeep = 5f;
        
        [field: SerializeField, ShowAssetPreview]
        public Sprite DefaultBackgroundImage { get; private set; }

        [SerializeField, SerializedDictionary("Level", "Data")]
        private SerializedDictionary<int, CaravanUpgradeData> caravanUpgradeDatas;

        public CaravanUpgradeData GetUpgradeData(int level)
        {
            var clampedLevel = Math.Clamp(level, 1, caravanUpgradeDatas.Count);
            return caravanUpgradeDatas[clampedLevel];
        }
    }
}