using System.Linq;
using Common.Infrastructure.Observation;
using Common.Utility;

namespace Features.Map.Modes
{
    public sealed class MapModeModel
    {
        public Observable<MapMode> MapMode { get; } = new();

        public void Toggle(MapMode mapMode)
        {
            MapMode.Value = MapMode.Value == mapMode ? Modes.MapMode.Default : mapMode;
        }

        public void Next()
        {
            var current = MapMode.Value;
            var mapModes = EnumExtensions.Enumerate<MapMode>().ToArray();
            var nextMode = ((int)current + 1) % mapModes.Length;
            MapMode.Value = (MapMode)nextMode;
        }
    }
}