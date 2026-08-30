using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Utility;
using DG.Tweening;
using Features.Ticking.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Effects
{
    public sealed class CloudShadow : MonoBehaviour
    {
        [SerializeField, Required]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private List<Sprite> cloudSprites;

        [SerializeField]
        private float maxAlpha = 0.30f, fadeInSeconds = 1.2f;

        private Tween _moveTween, _alphaTween;

        private readonly Bindings _bindings = new();

        private void Awake()
        {
            var gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _bindings.Track(
                gameSpeedModel.GameSpeed.Observe(OnGameSpeedChanged),
                gameSpeedModel.IsPaused.Observe(OnIsPausedChanged)
            );
        }

        private void OnDisable()
        {
            _bindings.Unbind();
        }

        private void OnIsPausedChanged(bool isPaused)
        {
            if (isPaused)
            {
                _alphaTween.Pause();
                _moveTween.Pause();
            }
            else
            {
                _alphaTween.Play();
                _moveTween.Play();
            }
        }

        private void OnGameSpeedChanged(GameSpeed speed)
        {
            var timeScale = speed == GameSpeed.Normal ? 1f : 2f;

            if (_moveTween != null)
            {
                _moveTween.timeScale = timeScale;
            }

            if (_alphaTween != null)
            {
                _alphaTween.timeScale = timeScale;
            }
        }

        public void Play(Vector3 startWorldPos, Vector3 endWorldPos, float moveSeconds)
        {
            transform.position = startWorldPos;
            spriteRenderer.sprite = cloudSprites.GetRandom();

            SetAlpha(0f);

            _alphaTween?.Kill();
            _alphaTween = spriteRenderer.DOFade(maxAlpha, fadeInSeconds).SetEase(Ease.InOutSine);


            _moveTween?.Kill();
            _moveTween = transform.DOMove(endWorldPos, moveSeconds).SetEase(Ease.Linear).OnComplete(BeginFadeOut);
        }

        private void BeginFadeOut()
        {
            _alphaTween?.Kill();
            _alphaTween = spriteRenderer.DOFade(0f, fadeInSeconds)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => Destroy(gameObject));
        }

        private void SetAlpha(float alpha)
        {
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

        private void OnDestroy()
        {
            _moveTween?.Kill();
            _alphaTween?.Kill();
        }
    }
}