namespace Features.Player.Logic
{
    public static class PlayerLocationExtension
    {
        public static bool IsInTown(this PlayerLocation location)
        {
            return location.CurrentTown != null;
        }
    }
}