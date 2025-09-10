using AYellowpaper.SerializedCollections;
using Data.Configuration;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = nameof(RecipeConfig), menuName = AssetMenu.ConfigDataFolder + nameof(RecipeConfig))]
    public sealed class RecipeConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Tier 1 Good", "Tier 2 Good")]
        public SerializedDictionary<Good, Good> Tier2Recipes { get; private set; }
        
        [field: SerializeField, SerializedDictionary("Tier 3 Good", "Components")]
        public SerializedDictionary<Good, Tier3Recipe> Tier3Recipes { get; private set; }
    }
}