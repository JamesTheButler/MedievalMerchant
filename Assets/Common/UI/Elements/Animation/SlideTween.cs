using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements.Animation
{
    public sealed class SlideTween : MonoBehaviour
    {
        [SerializeField, Required]
        private RectTransform target;

        [SerializeField]
        private Vector2 hiddenAnchoredPos, shownAnchoredPos;

        [SerializeField, Min(0f)]
        private float durationInSec = 0.5f;

        [SerializeField]
        private Ease easeIn = Ease.OutCubic;

        private Tween _tween;

        public void FadeIn()
        {
            _tween?.Kill();

            target.anchoredPosition = hiddenAnchoredPos;

            _tween = target
                .DOAnchorPos(shownAnchoredPos, durationInSec)
                .SetEase(easeIn)
                .SetUpdate(true)
                .SetLink(gameObject, LinkBehaviour.KillOnDisable);
        }

        public void FadeOut()
        {
            _tween?.Kill();
            _tween = null;
        }
    }
}