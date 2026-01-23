using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Map.Modes.UI
{
    public sealed class MapModeButtons : InitializableBehavior
    {
        [SerializeField]
        private MapMode mapMode;

        [SerializeField]
        private Color defaultColor, highlightColor;

        private Button _button;
        private MapModeModel _mapModeModel;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        public override void Initialize()
        {
            _mapModeModel = GameplayContext.Instance.Model.MapModeModel;
            _mapModeModel.MapMode.Observe(OnMapModeChanged);
        }

        public override void CleanUp()
        {
            _mapModeModel.MapMode.StopObserving(OnMapModeChanged);
        }

        private void OnMapModeChanged(MapMode speed)
        {
            UpdateButtonColor();
        }

        private void OnButtonClick()
        {
            var currentMode = _mapModeModel.MapMode.Value;
            _mapModeModel.MapMode.Value = currentMode == mapMode ? MapMode.Default : mapMode;
        }

        private void UpdateButtonColor()
        {
            var isThisSelected = _mapModeModel.MapMode.Value == mapMode;
            _button.image.color = isThisSelected ? highlightColor : defaultColor;
        }
    }
}