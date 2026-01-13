using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(fileName = nameof(GoodConfig), menuName = AssetMenu.ConfigDataFolder + nameof(GoodConfig))]
    public sealed class GoodConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, float> BasePriceData { get; private set; }

        [field: SerializeField]
        public float ForeignGoodPriceModifier { get; private set; }

        [field: SerializeField]
        public float LocalGoodPriceModifier { get; private set; }
    }
}