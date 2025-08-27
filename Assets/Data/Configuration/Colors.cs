using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = "Colors", menuName = AssetMenu.ConfigDataFolder + "Colors")]
    public sealed class Colors : ScriptableObject
    {
        [field: SerializeField]
        public Color FontDark { get; private set; }

        [field: SerializeField]
        public Color FontLight { get; private set; }

        [field: SerializeField]
        public Color Good { get; private set; }

        [field: SerializeField]
        public Color Bad { get; private set; }
    }
}