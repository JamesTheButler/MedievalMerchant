using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;
using Features.Player.Logic;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public sealed class MovementSpeedEffectLogic : EffectLogic<MovementSpeedEffectData>
    {
        private readonly PlayerModel _player;
        private readonly IModifier _modifier;

        public MovementSpeedEffectLogic(EffectOrigin effectOrigin, MovementSpeedEffectData effectData) :
            base(effectOrigin, effectData)
        {
            _player = GameplayContext.Instance.Model.Player;
            _modifier = new EffectPercentModifier(EffectData.SpeedBoostPercent, EffectOrigin);
        }

        public override void Apply()
        {
            _player.MovementSpeed.AddModifier(_modifier);
        }

        public override void Unapply()
        {
            if (_modifier == null)
                return;

            _player.MovementSpeed.RemoveModifier(_modifier);
        }
    }
}