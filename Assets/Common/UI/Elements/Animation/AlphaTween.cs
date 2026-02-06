using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements.Animation
{
    public sealed class AlphaTween : MonoBehaviour
    {
        [SerializeField, Required]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private float targetAlpha, inDurationInSec = .5f;

        [SerializeField]
        private Ease easeIn = Ease.OutCubic;

        private Tween _tween;

        public void FadeIn()
        {
            _tween?.Kill();
            _tween = canvasGroup
                .DOFade(targetAlpha, inDurationInSec)
                .SetUpdate(true)
                .SetEase(easeIn);
        }

        public void FadeOut()
        {
            _tween?.Kill();
            canvasGroup.alpha = 0f;
        }
    }
}