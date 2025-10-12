using Common;
using Features.Player.Caravan.Config;

namespace Features.Player.Caravan.Logic
{
    public sealed class Cart
    {
        public Observable<int> Level { get; } = new();
        public Observable<int> SlotCount { get; } = new();
        public Observable<float> MoveSpeed { get; } = new();
        public Observable<float> Upkeep { get; } = new();

        public Cart(
            int level = 0,
            int slotCount = 0,
            float moveSpeed = 0,
            float upkeep = 0
        )
        {
            Level.Value = level;
            SlotCount.Value = slotCount;
            MoveSpeed.Value = moveSpeed;
            Upkeep.Value = upkeep;
        }

        public void Update(int level, CaravanUpgradeData upgradeData)
        {
            Level.Value = level;
            SlotCount.Value = upgradeData.SlotCount;
            MoveSpeed.Value = upgradeData.MoveSpeed;
            Upkeep.Value = upgradeData.Upkeep;
        }

        public static Cart Level0 => new();
    }
}