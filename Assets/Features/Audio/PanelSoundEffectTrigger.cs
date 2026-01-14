using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Audio
{
    public sealed class PanelSoundEffectTrigger : InitializableBehavior
    {
        [SerializeField]
        private bool autoFindPanel;

        [SerializeField, Required]
        private DynamicPanel panel;

        private SfxService _sfxService;

        public override void Initialize()
        {
            if (autoFindPanel)
            {
                panel = FindFirstObjectByType<DynamicPanel>();
            }

            panel.Opened += OnOpened;
            panel.Closed += OnClosed;
        }

        private void OnOpened()
        {
            _sfxService.UISoundEffect.Invoke(UISoundEffect.PanelOpened);
        }

        private void OnClosed()
        {
            _sfxService.UISoundEffect.Invoke(UISoundEffect.PanelClosed);
        }
    }
}