using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
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
        public ObservableEvent CartUnlocked { get; } = new();

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
            _caravanConfig = ConfigurationManager.Configurations.CaravanConfig;
            var loc = ResourceManager.Instance.LocalizationResources.Player;
            _averageSpeedModifier = new AverageBaseValueModifier(loc.MovementSpeed);
            MoveSpeed = new ModifiableVariable(
                loc.MovementSpeed,
                true,
                _averageSpeedModifier);

            Upkeep = new ModifiableVariable(
                loc.CaravanUpkeep,
                false,
                new BaseUpkeepModifier(_caravanConfig.BaseUpkeep));

            for (var i = 0; i < CaravanConfig.MaxCartCount; i++)
            {
                var cart = new Cart();
                _carts.Add(cart);

                cart.SlotCount.Observe(SlotCountChanged);
            }
        }

        public void UpgradeCart(int cartId)
        {
            if (cartId is >= CaravanConfig.MaxCartCount or < 0)
            {
                Debug.LogError($"Invalid index: {cartId}. There are only {CaravanConfig.MaxCartCount} carts.");
                return;
            }

            var nextLevel = _carts[cartId].Level + 1;
            if (nextLevel is > CaravanConfig.MaxLevel or < 0)
            {
                Debug.LogError($"Invalid level: {nextLevel}. Max. level is {CaravanConfig.MaxLevel}.");
                return;
            }

            var upgradeData = _caravanConfig.GetUpgradeData(nextLevel);
            var cart = _carts[cartId];
            var oldLevel = cart.Level.Value;
            if (oldLevel == 0 && nextLevel > 0)
            {
                _averageSpeedModifier.AddValue(cart.MoveSpeed);
            }

            _carts[cartId].Update(nextLevel, upgradeData);
            RefreshTotals(cartId);

            if (oldLevel == 0 && nextLevel > 0)
            {
                CartUnlocked.Invoke();
            }
        }

        private void SlotCountChanged(int oldCount, int newCount)
        {
            SlotCount.Value += -oldCount + newCount;
        }

        private void RefreshTotals(int cartId)
        {
            var modifier = _cartUpkeepModifiers[cartId];
            var cart = _carts[cartId];
            if (modifier is null)
            {
                var displayIndex = cartId + 1;
                var newModifier = new CartUpkeepModifier(displayIndex, cart.Upkeep, cart.Level);
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