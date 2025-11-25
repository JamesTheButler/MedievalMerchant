using System.Linq;
using UnityEngine;

namespace Features.Map
{
    public static class ProductionZoneExtensions
    {
        public static bool IsAdjacentTo(this ProductionZone zone, Vector2Int position, float distanceThreshold)
        {
            return zone.Points.Any(point => Vector2.Distance(position, point + zone.Position) <= distanceThreshold);
        }
    }
}