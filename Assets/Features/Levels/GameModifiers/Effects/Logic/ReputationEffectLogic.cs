using Common.Infrastructure.Gameplay;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public sealed class ReputationEffectLogic : EffectLogic<ReputationEffectData>
    {
        private readonly GameplayModel _gameplayModel;
        private readonly EffectPercentModifier _modifier;

        public ReputationEffectLogic(EffectOrigin effectOrigin, ReputationEffectData effectData)
            : base(effectOrigin, effectData)
        {
            _gameplayModel = GameplayContext.Instance.Model;
            _modifier = new EffectPercentModifier(effectData.ReputationBoostPercent, effectOrigin);
        }

        public override void Apply()
        {
            foreach (var town in _gameplayModel.Towns.Values)
            {
                town.ReputationModel.AddModifier(_modifier);
            }
        }

        public override void Unapply()
        {
            foreach (var town in _gameplayModel.Towns.Values)
            {
                town.ReputationModel.RemoveModifier(_modifier);
            }
        }
    }
}