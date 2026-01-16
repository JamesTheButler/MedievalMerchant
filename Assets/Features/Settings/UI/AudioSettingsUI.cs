using Common.Infrastructure.Global;
using Common.UI.Elements;
using Features.Settings.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Settings.UI
{
    public sealed class AudioSettingsUI : InitializableBehavior
    {
        [SerializeField, Required]
        private Slider volumeSlider, musicVolumeSlider, sfxVolumeSlider, uiVolumeSlider;

        [SerializeField, Required]
        private SettingsSliderGroup
            volumeSliderGroup,
            musicVolumeSliderGroup,
            sfxVolumeSliderGroup,
            uiVolumeSliderGroup;

        private AudioSettingsModel _audioSettings;

        public override void Initialize()
        {
            _audioSettings = GlobalContext.Instance.Model.AudioSettingsModel;

            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderChanged);
            uiVolumeSlider.onValueChanged.AddListener(OnUiVolumeSliderChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeSliderChanged);

            volumeSlider.value = _audioSettings.MasterVolume;
            musicVolumeSlider.value = _audioSettings.MusicVolume;
            uiVolumeSlider.value = _audioSettings.InterfaceVolume;
            sfxVolumeSlider.value = _audioSettings.SfxVolume;

            // force update texts on initialization
            volumeSliderGroup.UpdateText(_audioSettings.MasterVolume);
            musicVolumeSliderGroup.UpdateText(_audioSettings.MusicVolume);
            uiVolumeSliderGroup.UpdateText(_audioSettings.InterfaceVolume);
            sfxVolumeSliderGroup.UpdateText(_audioSettings.SfxVolume);
        }

        private void OnVolumeSliderChanged(float volume)
        {
            _audioSettings.MasterVolume.Value = (int)volume;
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