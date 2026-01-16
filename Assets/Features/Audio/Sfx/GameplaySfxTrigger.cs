using Common.Infrastructure.Global;
using Common.UI.Elements;
using UnityEngine;

namespace Features.Audio.Sfx
{
    public sealed class GameplaySfxTrigger : InitializableBehavior
    {
        [SerializeField]
        private GameSoundEffect soundEffect;

        private SfxService _sfxService;

        public override void Initialize()
        {
            _sfxService = GlobalContext.Instance.Services.SfxService;
        }

        public void PlaySound()
        {
            _sfxService.GameSoundEffect.Invoke(soundEffect);
        }
    }
}