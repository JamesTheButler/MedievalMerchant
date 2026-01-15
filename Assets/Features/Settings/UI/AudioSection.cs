using Common.Infrastructure;
using Common.UI.Elements;
using Features.Settings.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Settings.UI
{
    public sealed class AudioSection : InitializableBehavior
    {
        [SerializeField, Required]
        private Slider volumeSlider, musicVolumeSlider, sfxVolumeSlider, uiVolumeSlider;

        private AudioSettingsModel _audioSettings;

        public override void Initialize()
        {
            _audioSettings = GlobalContext.Instance.AudioSettingsModel;

            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChanged);
            uiVolumeSlider.onValueChanged.AddListener(OnUiVolumeSliderChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeSliderChanged);

            volumeSlider.value = _audioSettings.TotalVolume;
            musicVolumeSlider.value = _audioSettings.MusicVolume;
            uiVolumeSlider.value = _audioSettings.InterfaceVolume;
            sfxVolumeSlider.value = _audioSettings.SfxVolume;
        }

        private void OnVolumeSliderChanged(float volume)
        {
            _audioSettings.TotalVolume.Value = (int)volume;
        }

        private void OnMusicVolumeSliderChanged(float volume)
        {
            _audioSettings.MusicVolume.Value = (int)volume;
        }

        private void OnUiVolumeSliderChanged(float volume)
        {
            _audioSettings.InterfaceVolume.Value = (int)volume;
        }

        private void OnSfxVolumeSliderChanged(float volume)
        {
            _audioSettings.SfxVolume.Value = (int)volume;
        }
    }
}