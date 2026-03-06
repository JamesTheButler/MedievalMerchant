using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Trade.Haggling.Data
{
    [CreateAssetMenu(
        fileName = nameof(HaggleResources),
        menuName = AssetMenu.ResourceFolder + nameof(HaggleResources))]
    public sealed class HaggleResources : ScriptableObject
    {
        [SerializeField, SerializedDictionary]
        private SerializedDictionary<HaggleLevel, LocalizedString> haggleLevelNames, haggleLevelDescriptions;

        public string GetName(HaggleLevel level)
        {
            return haggleLevelNames[level].GetLocalizedString();
        }

        public string GetDescription(HaggleLevel level)
        {
            return haggleLevelDescriptions[level].GetLocalizedString();
        }
    }
}