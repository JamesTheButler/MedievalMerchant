using Common.Infrastructure.Global;
using Common.UI.Elements;
using Common.UI.Elements.Panels;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Audio.Sfx
{
    public sealed class DynamicPanelSfxTrigger : InitializableBehavior
    {
        [SerializeField, Required]
        private DynamicPanel panel;

        private SfxService _sfxService;

        public override void Initialize()
        {
            _sfxService = GlobalContext.Instance.Services.SfxService;

            if (!panel)
                return;

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