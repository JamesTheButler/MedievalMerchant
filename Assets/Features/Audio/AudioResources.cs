using AYellowpaper.SerializedCollections;
using Common.Utility;
using Features.Audio.Sfx;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Audio
{
    [CreateAssetMenu(
        fileName = nameof(AudioResources),
        menuName = AssetMenu.ResourceFolder + nameof(AudioResources))]
    public sealed class AudioResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<GameSoundEffect, AudioClip> GameSoundClips { get; private set; }

        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<UISoundEffect, AudioClip> UiSoundClips { get; private set; }

        [field: SerializeField, Required]
        public AudioClip StartMenuMusic { get; private set; }

        [field: SerializeField]
        public AudioClip[] GameplayMusic { get; private set; }
    }
}