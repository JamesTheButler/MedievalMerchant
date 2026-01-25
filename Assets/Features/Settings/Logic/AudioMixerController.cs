using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Common.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Features.Settings.Logic
{
    public sealed class AudioMixerController : InitializableBehavior
    {
        [SerializeField, Required]
        private AudioMixer audioMixer;

        private const float MinVolumeDb = -80f;

        private readonly Bindings _bindings = new();

        private AudioSettingsModel _audioSettings;

        public override void Initialize()
        {
            _audioSettings = GlobalContext.Instance.Model.AudioSettingsModel;

            _bindings.Track(
                _audioSettings.MasterVolume.Observe(MasterVolumeChanged),
                _audioSettings.MusicVolume.Observe(MusicVolumeChanged),
                _audioSettings.InterfaceVolume.Observe(InterfaceVolumeChanged),
                _audioSettings.SfxVolume.Observe(SfxVolumeChanged)
            );
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.UnbindAll();
        }

        private void MasterVolumeChanged(int volume)
        {
            SetVolume("MasterVolume", volume);
        }

        private void MusicVolumeChanged(int volume)
        {
            SetVolume("MusicVolume", volume);
        }

        private void InterfaceVolumeChanged(int volume)
        {
            SetVolume("InterfaceVolume", volume);
        }

        private void SfxVolumeChanged(int volume)
        {
            SetVolume("SfxVolume", volume);
        }

        private void SetVolume(string exposedParameter, int volume)
        {
            var volume01 = volume / 100f;
            float volumeDb;
            if (volume01.IsApproximately(0))
            {
                volumeDb = MinVolumeDb;
            }
            else
            {
                volumeDb = Mathf.Log10(volume01) * 20f;
            }

            audioMixer.SetFloat(exposedParameter, volumeDb);
        }
    }
}