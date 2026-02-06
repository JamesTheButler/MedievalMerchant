using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements
{
    public sealed class DynamicPanelAlphaTweener : InitializableBehavior
    {
        [SerializeField, Required]
        private DynamicPanel dynamicPanel;

        [SerializeField, Required]
        private AlphaTween alphaTween;

        public override void Initialize()
        {
            dynamicPanel.Opened += OnPanelOpened;
            dynamicPanel.Closed += OnPanelClosed;
        }

        private void OnPanelOpened()
        {
            alphaTween.TweenIn();
        }

        private void OnPanelClosed()
        {
            alphaTween.TweenOut();
        }
    }
}