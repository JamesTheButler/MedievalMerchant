using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.Flags.Config
{
    [CreateAssetMenu(
        fileName = nameof(FlagResources),
        menuName = AssetMenu.ResourceFolder + nameof(FlagResources))]
    public sealed class FlagResources : ScriptableObject
    {
        public sealed record Data(Sprite Flag, Sprite RegionIcon, Color IconColor);

        [SerializeField, Required]
        private Sprite sampleFlag;

        [SerializeField, Required]
        private Texture2D flags;

        [SerializeField, SerializedDictionary("Region", "Icon")]
        private SerializedDictionary<Region, Sprite> regionIcons;

        [SerializeField, SerializedDictionary("Flag Color", "Icon Color")]
        private SerializedDictionary<FlagColor, Color> goodIconColor;

        private readonly Dictionary<Vector2Int, Sprite> _cache = new();

        public Data GetData(FlagInfo info)
        {
            return new Data(
                GetFlagSprite(info.Color, info.Shape),
                regionIcons[info.Region],
                goodIconColor[info.Color]);
        }

        private Sprite GetFlagSprite(FlagColor color, FlagShape shape)
        {
            var index = new Vector2Int((int)shape, (int)color);

            if (_cache.TryGetValue(index, out var flagSprite))
                return flagSprite;

            var tileSize = sampleFlag.rect.size;
            var pixelsPerUnit = sampleFlag.pixelsPerUnit;

            var pos = new Vector2(index.x * tileSize.x, index.y * tileSize.y);
            var spriteRect = new Rect(pos, tileSize);
            var pivot = new Vector2(0.5f, 0.5f);

            var sprite = Sprite.Create(flags, spriteRect, pivot, pixelsPerUnit);
            _cache.Add(index, sprite);
            return sprite;
        }
    }
}