using System.Collections.Generic;
using Common.Infrastructure;
using Features.Levels.GameModifiers.Data;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Effects.Logic;

namespace Features.Levels.GameModifiers.Logic
{
    public sealed class GameModifierService : IService
    {
        private readonly Dictionary<(GameModifierData, EffectData), IEffectLogic> _logics = new();

        public void Initialize() { }
        public void CleanUp() { }

        public void ApplyModifier(GameModifierData modifierData)
        {
            var origin = modifierData switch
            {
                EventGameModifierData eventData => new EffectOrigin("Event", eventData.Title),
                LevelGameModifierData conditionData => new EffectOrigin("Level Condition", conditionData.Title),
                _ => new EffectOrigin("Unknown", "Unknown")
            };

            foreach (var effect in modifierData.Effects)
            {
                var logic = GetLogic(effect, origin);
                logic.Apply();
                _logics.Add((modifierData, effect), logic);
            }
        }

        public void RemoveModifier(GameModifierData modifierData)
        {
            foreach (var effect in modifierData.Effects)
            {
                var key = (data: modifierData, effect);
                if (!_logics.TryGetValue(key, out var logic))
                    continue;

                logic.Unapply();
                _logics.Remove(key);
            }
        }

        private static IEffectLogic GetLogic(EffectData data, EffectOrigin origin)
        {
            return data switch
            {
                MovementSpeedEffectData effectData => new MovementSpeedEffectLogic(origin, effectData),
                ProductionEffectData effectData => new ProductionEffectLogic(origin, effectData),
                ReputationEffectData effectData => new ReputationEffectLogic(origin, effectData),
                PriceEffectData effectData => new PriceEffectLogic(origin, effectData)
            };
        }
    }
}