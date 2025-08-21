using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "TownSetupInfo", menuName = "Data/TownSetupInfo")]
    public class TownSetupInfo : ScriptableObject
    {
        [field: SerializeField] public int InitialFunds { get; private set;} 
        [field: SerializeField] public ProductionTable Production { get; private set;} 
        [field: SerializeField] public TownNameGenerator TownName { get; private set;} 
    }
}