using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;

namespace Features.Towns
{
    public sealed class GlobalSurplusSystem : ISystem
    {
        private readonly Bindings _bindings = new();

        private readonly Dictionary<Good, int> _globalAmounts = new();
        private readonly Dictionary<Good, IModifier> _modifiers = new();

        private GameplayModel _gameplayModel;
        private LostInterestPriceModifier _modifier;
        private Town[] _towns;

        public void Initialize()
        {
            _gameplayModel = GameplayContext.Instance.Model;
            _towns = _gameplayModel.Towns.Values.ToArray();

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
        }
    }
}