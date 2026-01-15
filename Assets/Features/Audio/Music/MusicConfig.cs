using Common.Utility;
using UnityEngine;

namespace Features.Audio.Music
{
    [CreateAssetMenu(
        fileName = nameof(MusicConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(MusicConfig))]
    public sealed class MusicConfig : ScriptableObject
    {
        [field: SerializeField]
        public float SecondsBetweenSongs { get; private set; } = 5;

        [field: SerializeField]
        public int MinGapBetweenRepeats { get; private set; } = 1;
    }
}