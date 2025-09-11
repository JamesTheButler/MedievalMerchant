using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(TownNameGenerator),
        menuName = AssetMenu.ConfigDataFolder + nameof(TownNameGenerator))]
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