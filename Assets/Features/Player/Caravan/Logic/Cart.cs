using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Inventory;
using Features.Player.Caravan.Config;

namespace Features.Player.Caravan.Logic
{
    public sealed class Cart
    {
        public Observable<int> Level { get; } = new();
        public Observable<int> SlotCount { get; } = new();
        public Observable<float> MoveSpeed { get; } = new();
        public Observable<float> Upkeep { get; } = new();

        public ModifiableVariable UpgradeCost { get; }

        public Observable<InventoryEntry>[] Slots { get; } = new Observable<InventoryEntry>[4];

        private readonly CartUpgradeBaseCostModifier _baseCostModifier;

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

            for (var i = 0; i < Slots.Length; i++)
            {
                Slots[i] = new Observable<InventoryEntry>();
            }

            _baseCostModifier = new CartUpgradeBaseCostModifier(level + 1);
            var loc = ResourceManager.Instance.LocalizationResources.Player;
            UpgradeCost = new ModifiableVariable(loc.UpgradeCost, false, _baseCostModifier);
        }

        public void Update(int level, CaravanUpgradeData upgradeData)
        {
            _baseCostModifier.Update(level + 1);

            Level.Value = level;
            SlotCount.Value = upgradeData.SlotCount;
            MoveSpeed.Value = upgradeData.MoveSpeed;
            Upkeep.Value = upgradeData.Upkeep;
        }

        public void UpdateSlot(int slotIndex, Good good, int amount)
        {
            Slots[slotIndex].Value = new InventoryEntry(good, amount);
        }

        public void ClearSlot(int slotIndex)
        {
            Slots[slotIndex].Value = null;
        }
    }
}