using Data.Towns.Production.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = nameof(TownSetupInfo), menuName = AssetMenu.ConfigDataFolder + nameof(TownSetupInfo))]
    public sealed class TownSetupInfo : ScriptableObject
    {
        [field: SerializeField]
        public int InitialFunds { get; private set; }

        [field: SerializeField, Required]
        public ProductionTable Production { get; private set; }

        [field: SerializeField, Required]
        public TownNameGenerator NameGenerator { get; private set; }
    }
}