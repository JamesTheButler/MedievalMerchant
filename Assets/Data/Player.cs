namespace Data
{
    public sealed class Player
    {
        public PlayerInventory Inventory { get; private set; } = new();

        public PlayerUpgrades Upgrades { get; private set; } = PlayerUpgrades.None;

        public Player(int startFunds)
        {
            Inventory.AddFunds(startFunds);
        }
    }
}