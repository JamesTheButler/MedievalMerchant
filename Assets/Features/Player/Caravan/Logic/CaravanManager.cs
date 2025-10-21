using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Modifiable;
using Features.Player.Caravan.Config;
using UnityEngine;

namespace Features.Player.Caravan.Logic
{
    public sealed class CaravanManager
    {
        public ModifiableVariable Upkeep { get; }
        public ModifiableVariable MoveSpeed { get; }

        public Observable<int> SlotCount { get; } = new();
        public IReadOnlyList<Cart> Carts => _carts;

        private readonly List<Cart> _carts = new();
        private readonly CaravanConfig _caravanConfig;

        private readonly AverageBaseValueModifier _averageSpeedModifier;

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
            _averageSpeedModifier = new AverageBaseValueModifier("Movement Speed");
            MoveSpeed = new ModifiableVariable(
                "Movement Speed",
                _averageSpeedModifier,
                true);

            Upkeep = new ModifiableVariable(
                "Caravan Upkeep (not implemented yet)",
                new BaseUpkeepModifier(_caravanConfig.BaseUpkeep));

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
            var cart = _carts[cartId];
            var oldLevel = cart.Level.Value;
            if (oldLevel == 0 && level > 0)
            {
                _averageSpeedModifier.AddValue(cart.MoveSpeed);
            }

            _carts[cartId].Update(level, upgradeData);
            RefreshTotals(cartId);
        }

        private void RefreshTotals(int cartId)
        {
            var modifier = _cartUpkeepModifiers[cartId];
            var cart = _carts[cartId];
            if (modifier is null)
            {
                var newModifier = new CartUpkeepModifier(cart.Upkeep, cart.Level);
                _cartUpkeepModifiers[cartId] = newModifier;
                Upkeep.AddModifier(newModifier);
            }
            else
            {
                modifier.Update(cart.Upkeep, cart.Level);
            }
        }
    }
}