using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(fileName = nameof(Cursors), menuName = AssetMenu.ConfigDataFolder + nameof(Cursors))]
    public sealed class Cursors : ScriptableObject
    {
        [field: SerializeField]
        public Texture2D Default { get; private set; }

        [field: SerializeField]
        public Texture2D Pointer { get; private set; }
    }
}