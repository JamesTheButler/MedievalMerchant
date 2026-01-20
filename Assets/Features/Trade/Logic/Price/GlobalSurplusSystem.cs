using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Goods.Selector;
using Features.Towns;

namespace Features.Trade.Logic.Price
{
    public sealed class GlobalSurplusSystem : ISystem
    {
        private readonly Bindings _bindings = new();

        private readonly Dictionary<Good, int> _globalAmounts = new();
        private readonly Dictionary<Good, GlobalSurplusPriceModifier> _modifiers = new();

        private GlobalSurplusModiferConfigData _config;
        private GameplayModel _gameplayModel;
        private Town[] _towns;

        public void Initialize()
        {
            _gameplayModel = GameplayContext.Instance.Model;
            _towns = _gameplayModel.Towns.Values.ToArray();
            _config = ConfigurationManager.Configurations.PriceModifierConfig.GlobalSurplusModiferConfig;

            foreach (var town in _towns)
            {
                _bindings.Track(
                    town.Inventory.GoodUpdatedWithOld.Observe(OnGoodUpdated)
                );
            }
        }

        public void CleanUp()
        {
            _bindings.UnbindAll();
        }

        private void OnGoodUpdated(Good good, int oldValue, int newValue)
        {
            if (!_globalAmounts.TryAdd(good, newValue))
            {
                _globalAmounts[good] += newValue - oldValue;
            }

            // if there is a modifier already, update it
            if (_modifiers.TryGetValue(good, out var modifier))
            {
                modifier.Update(_globalAmounts[good]);
            }
            else if (_globalAmounts[good] > _config.StartThreshold)
            {
                var newModifier = new GlobalSurplusPriceModifier(good, _globalAmounts[good]);
                _modifiers.Add(good, newModifier);
                foreach (var town in _towns)
                {
                    town.PriceManager.AddModifier(newModifier, new SingleGoodSelector(good), TradeType.Sell);
                }
            }
        }
    }
}