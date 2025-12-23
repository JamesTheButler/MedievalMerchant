using Common.Infrastructure;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public sealed class ReputationEffectLogic : EffectLogic<ReputationEffectData>
    {
        private readonly GameplayModel _gameplayModel;

        public ReputationEffectLogic(EffectOrigin effectOrigin, ReputationEffectData effectData)
            : base(effectOrigin, effectData)
        {
            _gameplayModel = GameplayContext.Instance.Model;
        }

        public override void Apply() { }

        public override void Unapply() { }
    }
}