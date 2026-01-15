using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public sealed class DevelopmentEffectLogic : EffectLogic<DevelopmentEffectData>
    {
        private readonly GameplayModel _gameplayModel;
        private readonly IModifier _modifier;

        public DevelopmentEffectLogic(EffectOrigin effectOrigin, DevelopmentEffectData effectData)
            : base(effectOrigin, effectData)
        {
            _gameplayModel = GameplayContext.Instance.Model;
            _modifier = new EffectPercentModifier(EffectData.DevelopmentBoostPercent, EffectOrigin);
        }

        public override void Apply()
        {
            foreach (var town in _gameplayModel.Towns.Values)
            {
                town.DevelopmentManager.DevelopmentTrend.AddModifier(_modifier);
            }
        }

        public override void Unapply()
        {
            foreach (var town in _gameplayModel.Towns.Values)
            {
                town.DevelopmentManager.DevelopmentTrend.RemoveModifier(_modifier);
            }
        }
    }
}