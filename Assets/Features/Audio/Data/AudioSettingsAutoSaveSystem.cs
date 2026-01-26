using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;

namespace Features.Audio.Data
{
    public sealed class AudioSettingsAutoSaveSystem : ISystem
    {
        private AudioSettingsModel _model;
        private AudioSettingsPersistenceService _persistenceService;

        private readonly Bindings _bindings = new();

        public void Initialize()
        {
            _model = GlobalContext.Instance.Model.AudioSettingsModel;
            _persistenceService = GlobalContext.Instance.PersistenceServices.AudioSettingsPersistenceService;

            _model.MasterVolume.Observe(OnAnyChanged, false);
            _model.MusicVolume.Observe(OnAnyChanged, false);
            _model.SfxVolume.Observe(OnAnyChanged, false);
            _model.InterfaceVolume.Observe(OnAnyChanged, false);
        }

        public void CleanUp()
        {
            _bindings.UnbindAll();
        }

        private void OnAnyChanged()
        {
            _persistenceService.Save(new AudioSettingSaveData(
                _model.MasterVolume,
                _model.MusicVolume,
                _model.SfxVolume,
                _model.InterfaceVolume
            ));
        }
    }
}