using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Common;
using Common.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Towns.Flags.Config
{
    [CreateAssetMenu(
        fileName = nameof(FlagConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(FlagConfig))]
    public sealed class FlagConfig : ScriptableObject
    {
        public sealed record Data(Sprite Flag, Sprite RegionIcon, Color GoodColor);

        [SerializeField]
        private Texture2D flags;

        [FormerlySerializedAs("goodIcons"), SerializeField, SerializedDictionary]
        private SerializedDictionary<Regions, Sprite> regionIcons;

        [SerializeField, SerializedDictionary]
        private SerializedDictionary<FlagColor, Color> goodIconColor;

        [SerializeField]
        private Sprite placeholder;

        [SerializeField]
        private int pixelsPerUnit = 16;

        [SerializeField]
        private Vector2Int tileSize = new(16, 16);

        private readonly Dictionary<Vector2Int, Sprite> _cache = new();

        public Data GetData(FlagInfo info)
        {
            return new Data(
                GetFlagSprite(info.Color, info.Shape),
                GetRegionIcon(info.Region),
                goodIconColor[info.Color]);
        }

        private Sprite GetFlagSprite(FlagColor color, FlagShape shape)
        {
            var index = new Vector2Int((int)shape, (int)color);

            if (_cache.TryGetValue(index, out var flagSprite))
                return flagSprite;

            var pos = new Vector2(index.x * tileSize.x, index.y * tileSize.y);
            var spriteRect = new Rect(pos, tileSize);
            var pivot = new Vector2(0.5f, 0.5f);

            var sprite = Sprite.Create(flags, spriteRect, pivot, pixelsPerUnit);
            _cache.Add(index, sprite);
            return sprite;
        }

        private Sprite GetRegionIcon(Regions region)
        {
            foreach (var (_, sprite) in regionIcons.Where(key => region.HasFlag(key.Key)))
            {
                return sprite;
            }

            return regionIcons[Regions.Forest];
        }
    }
}