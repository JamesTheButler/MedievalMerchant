using Common;
using Features.Towns.Config;
using Features.Towns.Production.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns
{
    [CreateAssetMenu(fileName = nameof(TownSetupInfo), menuName = AssetMenu.ConfigDataFolder + nameof(TownSetupInfo))]
    public sealed class TownSetupInfo : ScriptableObject
    {
        [field: SerializeField]
        public float InitialFunds { get; private set; }

        [field: SerializeField, Required]
        public ProductionTable Production { get; private set; }

        [field: SerializeField, Required]
        public TownNameGenerator NameGenerator { get; private set; }
    }
}