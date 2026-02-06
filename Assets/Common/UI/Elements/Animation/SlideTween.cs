using Common.Infrastructure;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements.Animation
{
    public sealed class SlideTween : InitializableBehavior
    {
        [SerializeField, Required]
        private RectTransform target;

        [SerializeField]
        private Vector2 hiddenAnchoredPos, shownAnchoredPos;

        [SerializeField]
        private Ease easeIn = Ease.OutCubic;

        private Tween _tween;

        private float _durationInSec;

        public override void Initialize()
        {
            _durationInSec = ResourceManager.Instance.AnimationResources.PanelSlideInDurationSeconds;
        }

        public void FadeIn()
        {
            _tween?.Kill();

            target.anchoredPosition = hiddenAnchoredPos;

            _tween = target
                .DOAnchorPos(shownAnchoredPos, _durationInSec)
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