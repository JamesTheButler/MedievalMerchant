using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(PlayerConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(PlayerConfig))]
    public sealed class PlayerConfig : ScriptableObject
    {
        [field: SerializeField]
        public float MovementSpeed { get; private set; }
    }
}