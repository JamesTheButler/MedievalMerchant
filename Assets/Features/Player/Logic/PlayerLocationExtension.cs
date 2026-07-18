using Features.Towns;

namespace Features.Player.Logic
{
    public static class PlayerLocationExtension
    {
        public static bool IsInTown(this PlayerLocation location)
        {
            return location.MapLocation.Value is Town;
        }

        public static bool IsAtCampsite(this PlayerLocation location)
        {
            return location.MapLocation.Value is Camp.Logic.Camp;
        }

        public static bool IsAtLocation(this PlayerLocation location)
        {
            return location.MapLocation.Value != null;
        }
    }
}
