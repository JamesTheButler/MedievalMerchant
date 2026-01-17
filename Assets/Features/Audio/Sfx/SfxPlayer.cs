using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Audio.Sfx
{
    public sealed class SfxPlayer : InitializableSingleton
    {
        private static SfxPlayer _instance;

        private readonly Bindings _bindings = new();

        private SfxService _sfxService;
        private AudioResources _audioResources;

        [SerializeField, Required]
        private AudioSource gameAudioSource, uiAudioSource;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameAudioSource);
        }

        protected override void OnInitialize()
        {
            _sfxService = GlobalContext.Instance.Services.SfxService;
            _audioResources = ResourceManager.Instance.AudioResources;

            _bindings.Track(
                _sfxService.GameSoundEffect.Observe(OnGameSoundEffect),
                _sfxService.UISoundEffect.Observe(OnUISoundEffect)
            );
        }

        private void OnDestroy()
        {
            _bindings.UnbindAll();
        }

        private void OnUISoundEffect(UISoundEffect effect)
        {
            if (!uiAudioSource || !uiAudioSource.enabled)
                return;

            if (!_audioResources.UiSoundClips.TryGetValue(effect, out var audioClip) || audioClip == null)
            {
                Debug.Log($"No audio clip added for effect '{effect}'.");
                return;
            }

            uiAudioSource.PlayOneShot(audioClip);
        }

        private void OnGameSoundEffect(GameSoundEffect effect)
        {
            if (!gameAudioSource || !gameAudioSource.enabled)
                return;

            if (!_audioResources.GameSoundClips.TryGetValue(effect, out var audioClip) || audioClip == null)
            {
                Debug.Log($"No audio clip added for effect '{effect}'.");
                return;
            }

            gameAudioSource.PlayOneShot(audioClip);
        }
    }
}