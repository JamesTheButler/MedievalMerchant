namespace Data
{
    public sealed class Player
    {
        public PlayerInventory Inventory { get; private set; } = new();

        public Player(int startFunds)
        {
            Inventory.AddFunds(startFunds);
        }
    }
}