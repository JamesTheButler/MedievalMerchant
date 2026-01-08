using Common.Types;
using Common.Utility;
using Features.Towns.Flags.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.Flags.Test
{
    public sealed class FlagTestGrid : MonoBehaviour
    {
        [SerializeField]
        private Transform flagContainer;

        [SerializeField]
        private GameObject flagPrefab;

        [Button("Re-Generate Grid")]
        private void Generate()
        {
            var permutations = EnumExtensions.GetPermutations<FlagColor, FlagShape>();
            foreach (var (color, shape) in permutations)
            {
                var flagObject = Instantiate(flagPrefab, flagContainer);
                var flag = flagObject.GetComponent<FlagRenderer>();
                flag.name = $"Flag_{color}_{shape}";

                var info = new FlagInfo(color, shape, EnumExtensions.GetRandom<Region>());
                flag.SetFlag(info);
            }
        }
    }
}