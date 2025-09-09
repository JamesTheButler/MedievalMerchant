using AYellowpaper.SerializedCollections;
using Data.Configuration;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = nameof(RecipeConfig), menuName = AssetMenu.ConfigDataFolder + nameof(RecipeConfig))]
    public sealed class RecipeConfig : ScriptableObject
    {
        [field: SerializeField]
        public SerializedDictionary<Good, Good> Tier2Recipes { get; private set; }
    }
}