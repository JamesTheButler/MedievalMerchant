using NaughtyAttributes;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "TownSetupInfo", menuName = "Data/TownSetupInfo")]
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