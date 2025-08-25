using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Table to define which amount of different tier goods results in which development trend modifiers.
    /// </summary>
    [CreateAssetMenu(fileName = "DevelopmentTable", menuName = "Data/DevelopmentTable")]
    public class DevelopmentTable : ScriptableObject
    {
        [field: SerializeField] public List<int> Tier1Trends { get; private set; }
        [field: SerializeField] public List<int> Tier2Trends { get; private set; }
        [field: SerializeField] public List<int> Tier3Trends { get; private set; }
    }
}