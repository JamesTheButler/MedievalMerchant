using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "TownSetupInfo", menuName = "Data/TownSetupInfo")]
    public class TownSetupInfo : ScriptableObject
    {
        [SerializeField] private string townName;
        [SerializeField] private List<Good> production;
    }
}