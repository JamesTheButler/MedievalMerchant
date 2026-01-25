using System;
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
    public sealed class MusicPlayer : InitializableSingleton
    {
        private static MusicPlayer _instance;

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

        private bool _hasFocus = true;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            this.StopCoroutineSafe(_gameplayLoop);
            _bindings.UnbindAll();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _hasFocus = hasFocus;
        }

        protected override void OnInitialize()
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

        private void SetMusicMode(MusicMode mode)
        {
            if (_currentMode == mode)
                return;

            if (!audioSource || !audioSource.enabled)
                return;

            _currentMode = mode;


            switch (mode)
            {
                case MusicMode.Menu:
                    audioSource.Stop();
                    this.StopCoroutineSafe(_gameplayLoop);
                    audioSource.clip = _audioResources.StartMenuMusic;
                    audioSource.loop = true;
                    Debug.Log($"{nameof(MusicPlayer)}: Now playing {_audioResources.StartMenuMusic.name}.");
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
                while (true)
                {
                    while (!_hasFocus)
                        yield return null;

                    if (!audioSource.isPlaying)
                        break;

                    yield return null;
                }

                yield return new WaitForSeconds(_musicConfig.SecondsBetweenSongs);

                audioSource.Stop();
                var nextSong = _activePool.GetRandom();
                audioSource.clip = nextSong;
                audioSource.Play();
                Debug.Log($"{nameof(MusicPlayer)}: Now playing {nextSong.name}.");

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