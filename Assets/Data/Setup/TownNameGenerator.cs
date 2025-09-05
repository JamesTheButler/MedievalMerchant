using System.Collections.Generic;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "TownNameGenerator", menuName = "Data/TownNameGenerator")]
    public sealed class TownNameGenerator : ScriptableObject
    {
        [SerializeField]
        private List<string> prefixes;

        [SerializeField]
        private List<string> suffixes;

        public string GenerateName()
        {
            return prefixes.GetRandom() + suffixes.GetRandom();
        }
    }
}