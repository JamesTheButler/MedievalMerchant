using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public sealed class ProductionEffectLogic : EffectLogic<ProductionEffectData>
    {
        private readonly GameplayModel _gameplayModel;
        private readonly IModifier _modifier;

        public ProductionEffectLogic(EffectOrigin effectOrigin, ProductionEffectData effectData)
            : base(effectOrigin, effectData)
        {
            _gameplayModel = GameplayContext.Instance.Model;
            _modifier = new EffectPercentModifier(EffectData.ProductionBoostPercent, EffectOrigin);
        }

        public override void Apply()
        {
            foreach (var town in _gameplayModel.Towns.Values)
            {
                town.ProductionManager.AddModifier(_modifier, EffectData.Selector.Selector);
            }
        }

        public override void Unapply()
        {
            foreach (var town in _gameplayModel.Towns.Values)
            {
                town.ProductionManager.RemoveModifier(_modifier, EffectData.Selector.Selector);
            }
        }
    }
}