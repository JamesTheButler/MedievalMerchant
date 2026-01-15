using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Audio.Sfx
{
    public sealed class SfxPlayer : InitializableBehavior
    {
        private readonly Bindings _bindings = new();

        private SfxService _sfxService;
        private AudioResources _audioResources;

        [SerializeField, Required]
        private AudioSource gameAudioSource, uiAudioSource;

        private void Awake()
        {
            DontDestroyOnLoad(gameAudioSource);
        }

        public override void Initialize()
        {
            _sfxService = GlobalContext.Instance.Services.SfxService;
            _audioResources = ResourceManager.Instance.AudioResources;

            _bindings.Track(
                _sfxService.GameSoundEffect.Observe(OnGameSoundEffect),
                _sfxService.UISoundEffect.Observe(OnUISoundEffect)
            );
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.UnbindAll();
        }

        private void OnUISoundEffect(UISoundEffect effect)
        {
            if (!_audioResources.UiSoundClips.TryGetValue(effect, out var audioClip) || audioClip == null)
            {
                Debug.LogWarning($"No audio clip added for effect '{effect}'.");
                return;
            }

            uiAudioSource.PlayOneShot(audioClip);
        }

        private void OnGameSoundEffect(GameSoundEffect effect)
        {
            if (!_audioResources.GameSoundClips.TryGetValue(effect, out var audioClip) || audioClip == null)
            {
                Debug.LogWarning($"No audio clip added for effect '{effect}'.");
                return;
            }

            gameAudioSource.PlayOneShot(audioClip);
        }
    }
}