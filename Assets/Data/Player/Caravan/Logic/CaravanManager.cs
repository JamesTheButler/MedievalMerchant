using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Configuration;
using Data.Modifiable;
using Data.Player.Caravan.Config;
using UnityEngine;

namespace Data.Player.Caravan.Logic
{
    public sealed class CaravanManager
    {
        public ModifiableVariable TotalUpkeep { get; }
        public ModifiableVariable TotalMoveSpeed { get; }

        public Observable<int> SlotCount { get; } = new();
        public IReadOnlyList<Cart> Carts => _carts;

        private readonly List<Cart> _carts = new();
        private readonly CaravanConfig _caravanConfig;

        private readonly List<CartUpkeepModifier> _cartUpkeepModifiers = new()
        {
            null,
            null,
            null,
            null,
        };

        public CaravanManager()
        {
            _caravanConfig = ConfigurationManager.Instance.CaravanConfig;
            TotalMoveSpeed = new ModifiableVariable(new AverageBaseValueModifier(0, "Movement Speed"));
            TotalUpkeep = new ModifiableVariable(new BaseUpkeepModifier(CaravanConfig.BaseUpkeep));

            for (var i = 0; i < CaravanConfig.MaxCartCount; i++)
            {
                var cart = new Cart();
                _carts.Add(cart);
                cart.SlotCount.Observe(SlotCountChanged);
            }
        }

        ~CaravanManager()
        {
            foreach (var cart in _carts)
            {
                cart.SlotCount.StopObserving(SlotCountChanged);
            }
        }

        private void SlotCountChanged(int oldCount, int newCount)
        {
            SlotCount.Value += -oldCount + newCount;
        }

        public void UpgradeCart(int cartId)
        {
            if (cartId is >= CaravanConfig.MaxCartCount or < 0)
            {
                Debug.LogError($"Invalid index: {cartId}. There are only {CaravanConfig.MaxCartCount} carts.");
                return;
            }

            UpgradeCart(cartId, _carts[cartId].Level + 1);
        }

        public void UpgradeCart(int cartId, int level)
        {
            if (cartId is >= CaravanConfig.MaxCartCount or < 0)
            {
                Debug.LogError($"Invalid index: {cartId}. There are only {CaravanConfig.MaxCartCount} carts.");
                return;
            }

            if (level is > CaravanConfig.MaxLevel or < 0)
            {
                Debug.LogError($"Invalid level: {level}. Max. level is {CaravanConfig.MaxLevel}.");
                return;
            }

            var upgradeData = _caravanConfig.GetUpgradeData(level);
            _carts[cartId].Update(level, upgradeData);

            RefreshTotals(cartId);
        }

        private void RefreshTotals(int cartId)
        {
            // TODO - CORE: when modifiers are observable, this will become a lot simpler
            TotalUpkeep.RemoveModifier(_cartUpkeepModifiers[cartId]);
            var cart = _carts[cartId];
            var newUpkeepModifier = new CartUpkeepModifier(cart.Upkeep, cart.Level);
            _cartUpkeepModifiers[cartId] = newUpkeepModifier;
            TotalUpkeep.AddModifier(newUpkeepModifier);

            // TODO - CORE: when modifiers are observable, this will become a lot simpler
            var averageMoveSpeedModifier = TotalMoveSpeed.Modifiers.FirstOfType<AverageBaseValueModifier, IModifier>();
            TotalMoveSpeed.RemoveModifier(averageMoveSpeedModifier);
            var averageMoveSpeed = _carts.Where(c => c.Level > 0).Average(c => c.MoveSpeed);
            TotalMoveSpeed.AddModifier(new AverageBaseValueModifier(averageMoveSpeed, "Movement Speed"));
        }
    }
}