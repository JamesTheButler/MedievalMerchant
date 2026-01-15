using System.Collections;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Common.Utility;
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
        private Coroutine _gameplayLoop;

        private readonly Bindings _bindings = new();
        private readonly List<AudioClip> _activePool = new();
        private readonly Queue<AudioClip> _inactivePool = new();

        private MusicMode? _currentMode;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public override void Initialize()
        {
            _audioResources = ResourceManager.Instance.AudioResources;
            _musicService = GlobalContext.Instance.Services.MusicService;
            _musicConfig = ConfigurationManager.Configurations.MusicConfig;

            _activePool.Clear();
            _activePool.AddRange(_audioResources.GameplayMusic);
            
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
            if (_currentMode == mode)
                return;

            _currentMode = mode;

            audioSource.Stop();

            switch (mode)
            {
                case MusicMode.Menu:
                    this.StopCoroutineSafe(_gameplayLoop);
                    audioSource.clip = _audioResources.StartMenuMusic;
                    audioSource.loop = true;
                    audioSource.Play();
                    break;
                case MusicMode.Gameplay:
                    _gameplayLoop = StartCoroutine(GameplayLoop());
                    break;
            }
        }

        private IEnumerator GameplayLoop()
        {
            audioSource.loop = false;

            while (true)
            {
                if (audioSource.isPlaying)
                    yield return null;

                yield return new WaitForSeconds(_musicConfig.SecondsBetweenSongs);

                var nextSong = _activePool.GetRandom();
                audioSource.clip = nextSong;
                audioSource.Play();

                if (_inactivePool.Count >= _musicConfig.MinGapBetweenRepeats)
                {
                    _activePool.Add(_inactivePool.Dequeue());
                }

                _inactivePool.Enqueue(nextSong);
                _activePool.Remove(nextSong);
            }
        }
    }
}