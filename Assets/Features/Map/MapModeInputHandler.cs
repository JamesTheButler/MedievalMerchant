using System.Linq;
using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using Common.Utility;
using UnityEngine.InputSystem;

namespace Features.Map
{
    public sealed class MapModeInputHandler : InitializableBehavior
    {
        private MapModeModel _mapModeModel;

        public override void Initialize()
        {
            _mapModeModel = GameplayContext.Instance.Model.MapModeModel;
        }

        public void OnMapModeDefaultKey(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            _mapModeModel.MapMode.Value = MapMode.Default;
        }

        public void OnMapModeTownKey(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            _mapModeModel.MapMode.Value = MapMode.Town;
        }

        public void OnMapModeZonesKey(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            _mapModeModel.MapMode.Value = MapMode.Zone;
        }

        public void OnMapModeCycleKey(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            var current = _mapModeModel.MapMode.Value;
            var mapModes = EnumExtensions.Enumerate<MapMode>().ToArray();
            var nextMode = ((int)current + 1) % mapModes.Length;
            _mapModeModel.MapMode.Value = (MapMode)nextMode;
        }
    }
}