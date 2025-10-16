using Common;
using Common.Types;
using Features.Towns.Flags.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.Flags.Test
{
    public sealed class FlagTestGrid : MonoBehaviour
    {
        [SerializeField]
        private ConfigurationManager configurationManager;

        [SerializeField]
        private Transform flagContainer;

        [SerializeField]
        private GameObject flagPrefab;

        [Button("Re-Generate Grid")]
        private void Generate()
        {
            foreach (Transform child in flagContainer)
            {
                DestroyImmediate(child.gameObject);
            }

            var permutations = EnumExtensions.GetPermutations<FlagColor, FlagShape>();
            foreach (var (color, shape) in permutations)
            {
                var flagObject = Instantiate(flagPrefab, flagContainer);
                var flag = flagObject.GetComponent<FlagUI>();
                flag.name = $"Flag_{color}_{shape}";

                var info = new FlagInfo(color, shape, Regions.Fields);
                flag.SetFlag(info);
            }
        }
    }
}