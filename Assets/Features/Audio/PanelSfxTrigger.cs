using Common.Infrastructure;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Audio
{
    public sealed class PanelSfxTrigger : InitializableBehavior
    {
        [SerializeField, Required]
        private DynamicPanel panel;

        private SfxService _sfxService;

        public override void Initialize()
        {
            _sfxService = GlobalContext.Instance.Services.SfxService;
            
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