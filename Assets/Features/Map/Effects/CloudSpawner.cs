using System.Collections;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Features.Map.Tiling;
using Features.Ticking.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Effects
{
    public sealed class CloudSpawner : InitializableBehavior
    {
        [SerializeField, Required]
        private CloudShadow cloudPrefab;

        [SerializeField, Required]
        private TilemapManager tilemapManager;

        [Header("Spawn")]
        [SerializeField]
        private float spawnIntervalSeconds = 6f;

        [SerializeField]
        private float paddingWorldUnits = 2f;

        [Header("Movement")]
        [SerializeField]
        private float baseSpeedUnitsPerSecond = 0.35f;

        [SerializeField]
        private Vector2 speedMultiplierRange = new(0.8f, 1.2f);

        [SerializeField]
        private float verticalPaddingWorldUnits = 0.5f;

        private bool _isInitialized, _isPaused;
        private float _nextSpawnTime, _timeScale = 1f;
        private Bounds _bounds;

        private readonly Bindings _bindings = new();

        public override void Initialize()
        {
            var gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;

            _bindings.Track(
                gameSpeedModel.GameSpeed.Observe(OnGameSpeedChanged),
                gameSpeedModel.IsPaused.Observe(OnIsPausedChanged)
            );

            StartCoroutine(InitCoroutine());
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.UnbindAll();
        }

        // 🤨
        private IEnumerator InitCoroutine()
        {
            yield return new WaitForEndOfFrame();
            _bounds = tilemapManager.Tilemap.localBounds;
            _isInitialized = true;
        }

        private void OnIsPausedChanged(bool isPaused)
        {
            _isPaused = isPaused;
        }

        private void OnGameSpeedChanged(GameSpeed speed)
        {
            _timeScale = speed != GameSpeed.Normal ? 1f : 2f;
        }

        private void Update()
        {
            if (!_isInitialized || _isPaused)
                return;

            if (Time.time < _nextSpawnTime)
                return;

            var effectiveInterval = spawnIntervalSeconds / _timeScale;
            _nextSpawnTime = Time.unscaledTime + effectiveInterval;
            SpawnCloud();
        }

        private void SpawnCloud()
        {
            var minX = _bounds.min.x;
            var maxX = _bounds.max.x;
            var minY = _bounds.min.y + verticalPaddingWorldUnits;
            var maxY = _bounds.max.y - verticalPaddingWorldUnits;

            var y = Random.Range(minY, maxY);

            var start = new Vector3(minX - paddingWorldUnits, y, transform.position.z);
            var end = new Vector3(maxX + paddingWorldUnits, y, transform.position.z);

            var speedMultiplier = Random.Range(speedMultiplierRange.x, speedMultiplierRange.y);
            var speed = baseSpeedUnitsPerSecond * speedMultiplier;

            var distance = Mathf.Abs(end.x - start.x);
            var moveSeconds = distance / Mathf.Max(0.0001f, speed);

            var cloud = Instantiate(cloudPrefab, transform);
            cloud.Play(start, end, moveSeconds);
        }
    }
}