using NaughtyAttributes;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "GoodInfo", menuName = "Data/GoodInfo")]
    public sealed class GoodConfigData : ScriptableObject
    {
        [field: SerializeField]
        public string GoodName { get; private set; }

        [field: SerializeField]
        public Tier Tier { get; private set; }

        [field: SerializeField, Required]
        public Sprite Icon { get; private set; }
    }
}