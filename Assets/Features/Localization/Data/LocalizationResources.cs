using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [CreateAssetMenu(
        fileName = nameof(LocalizationResources),
        menuName = AssetMenu.ResourceFolder + nameof(LocalizationResources))]
    public sealed class LocalizationResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Difficulty, LocalizedString> Difficulties { get; private set; }
        
        [field: SerializeField]
        public NotificationLocalizationResources NotificationResources { get; private set; }
    }
}