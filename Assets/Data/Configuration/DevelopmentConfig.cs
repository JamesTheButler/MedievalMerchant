using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = nameof(DevelopmentConfig), menuName = AssetMenu.ConfigDataFolder + nameof(DevelopmentConfig))]
    public sealed class DevelopmentConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, DevelopmentTable> DevelopmentTables { get; private set; }

        [field: SerializeField]
        public float DevelopmentMultiplier { get; private set; } = 1f;
    }
}