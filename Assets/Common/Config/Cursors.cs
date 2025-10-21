using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(fileName = nameof(Cursors), menuName = AssetMenu.ConfigDataFolder + nameof(Cursors))]
    public sealed class Cursors : ScriptableObject
    {
        [field: SerializeField]
        public CursorData Default { get; private set; }

        [field: SerializeField]
        public CursorData Pointer { get; private set; }
    }
}