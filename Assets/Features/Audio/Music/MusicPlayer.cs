using System.Collections;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Audio.Music
{
    public sealed class MusicPlayer : InitializableBehavior
    {
        private AudioResources _audioResources;

        [SerializeField, Required]
        private AudioSource audioSource;

        private MusicService _musicService;
        private MusicConfig _musicConfig;
        private Coroutine _gameplayLoopCoroutine;

        private Bindings _bindings;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public override void Initialize()
        {
            _audioResources = ResourceManager.Instance.AudioResources;
            _musicService = GlobalContext.Instance.Services.MusicService;
            _musicConfig = ConfigurationManager.Configurations.MusicConfig;

            _bindings.Track(
                _musicService.MusicModeChange.Observe(SetMusicMode)
            );
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.UnbindAll();

        }

        private void SetMusicMode(MusicMode mode)
        {
            if (_gameplayLoopCoroutine != null)
            {
                StopCoroutine(_gameplayLoopCoroutine);
            }
            
            // mode.Gameplay:
            // play song
            // wait x seconds
            // play next random song (but not previous two)
            if (audioSource.isPlaying)
            {
                audioSource.clip = _audioResources.StartMenuMusic;
                audioSource.Play();
            }

            audioSource.clip = _audioResources.StartMenuMusic;
            audioSource.Play();
        }

        private List<AudioClip> _activePool;
        private List<AudioClip> _inactivePool;
        
        private IEnumerator GameplayLoop()
        {
            
            while (true)
            {
                if (audioSource.isPlaying)
                    yield return null;

                if (_audioResources)
                {
                   // _musicConfig.
                }
            }
            yield return null;
        }
    }
}