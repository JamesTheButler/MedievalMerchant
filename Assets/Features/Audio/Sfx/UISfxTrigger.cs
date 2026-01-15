using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.UI.Elements;
using UnityEngine;

namespace Features.Audio.Sfx
{
    public sealed class UISfxTrigger : InitializableBehavior
    {
        [SerializeField]
        private UISoundEffect uiSoundEffect;

        private SfxService _sfxService;

        public override void Initialize()
        {
            _sfxService = GlobalContext.Instance.Services.SfxService;
        }

        public void PlaySound()
        {
            _sfxService.UISoundEffect.Invoke(uiSoundEffect);
        }
    }
}