using Common.UI.Elements.Animation;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements.Panels
{
    public sealed class DynamicPanelSlideTweener : InitializableBehavior
    {
        [SerializeField, Required]
        private DynamicPanel dynamicPanel;

        [SerializeField, Required]
        private SlideTween slideTween;

        public override void Initialize()
        {
            dynamicPanel.Opened += OnPanelOpened;
            dynamicPanel.Closed += OnPanelClosed;
        }

        private void OnPanelOpened()
        {
            slideTween.FadeIn();
        }

        private void OnPanelClosed()
        {
            slideTween.FadeOut();
        }
    }
}