namespace Data
{
    public class Player
    {
        public Inventory Inventory { get; private set; } = new();

        public Player(int startFunds)
        {
            Inventory.AddFunds(startFunds);
        }
    }
}