using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using UnityEngine.UI;

namespace Features.Map.Modes.UI
{
    public sealed class MapModeCycleButton : InitializableBehavior
    {
        private Button _button;
        private MapModeModel _mapModeModel;

        public override void Initialize()
        {
            _mapModeModel = GameplayContext.Instance.Model.MapModeModel;

            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _mapModeModel.Next();
        }
    }
}