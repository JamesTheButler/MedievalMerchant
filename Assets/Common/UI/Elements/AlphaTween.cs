using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements
{
    public sealed class AlphaTween : MonoBehaviour
    {
        [SerializeField, Required]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private float targetAlpha, inDurationInSec = .5f, outDurationInSec = 0.25f;

        [SerializeField]
        private Ease easeIn = Ease.OutCubic, easeOut = Ease.InCubic;

        private Tween _tween;

        public void TweenIn()
        {
            _tween?.Kill();
            _tween = canvasGroup
                .DOFade(targetAlpha, inDurationInSec)
                .SetEase(easeIn);
        }

        public void TweenOut()
        {
            _tween?.Kill();
            _tween = canvasGroup
                .DOFade(0, outDurationInSec)
                .SetEase(easeOut);
        }
    }
}