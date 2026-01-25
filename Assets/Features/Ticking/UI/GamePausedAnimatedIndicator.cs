using System.Collections;
using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using Features.Ticking.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Ticking.UI
{
    public sealed class GamePausedAnimatedIndicator : InitializableBehavior
    {
        [SerializeField, Required]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private float fadeDurationInSeconds;

        private GameSpeedModel _gameSpeedModel;
        private Coroutine _currentRoutine;

        public override void Initialize()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _gameSpeedModel.IsPaused.Observe(OnPausedChanged);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _gameSpeedModel.IsPaused.StopObserving(OnPausedChanged);
        }

        private void OnPausedChanged(bool isPaused)
        {
            if (_currentRoutine != null)
                StopCoroutine(_currentRoutine);

            _currentRoutine = StartCoroutine(Fade(isPaused ? 1f : 0f));
        }

        private IEnumerator Fade(float targetAlpha)
        {
            var startAlpha = canvasGroup.alpha;
            var time = 0f;

            while (time < fadeDurationInSeconds)
            {
                time += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDurationInSeconds);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}