using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements.Animation
{
    public sealed class BlinkTween : MonoBehaviour
    {
        [SerializeField, Required]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private float fadeDuration = 0.08f, startFadeAlpha = 0.8f;

        [SerializeField]
        private int blinkCount = 2;

        private Tween _tween;

        private void OnEnable()
        {
            canvasGroup.alpha = 1f;
            _tween?.Kill();
            _tween = canvasGroup
                .DOFade(startFadeAlpha, fadeDuration)
                .SetLoops(blinkCount * 2, LoopType.Yoyo)
                .SetUpdate(true)
                .SetLink(gameObject, LinkBehaviour.KillOnDisable);
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }
    }
}
