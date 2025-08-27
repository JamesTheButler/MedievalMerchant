using AYellowpaper.SerializedCollections;
using Data.Setup;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = "DevelopmentConfig", menuName = AssetMenu.ConfigDataFolder + "DevelopmentConfig")]
    public sealed class DevelopmentConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, DevelopmentTable> DevelopmentTables { get; private set; }

        [field: SerializeField]
        public float DevelopmentMultiplier { get; private set; } = 1f;
    }
}