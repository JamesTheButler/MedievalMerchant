using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Colors", menuName = "Data/Colors")]
    public class Colors : ScriptableObject
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