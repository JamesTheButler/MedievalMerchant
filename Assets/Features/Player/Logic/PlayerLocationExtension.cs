using Features.Map.Pathfinding;
using Features.Towns;

namespace Features.Player.Logic
{
    public static class PlayerLocationExtension
    {
        public static bool IsInTown(this PlayerLocation location)
        {
            return location.MapLocation.Value is Town;
        }

        public static bool IsAtLocation(this PlayerLocation location)
        {
            return location.MapLocation.Value != null;
        }
    }
}
