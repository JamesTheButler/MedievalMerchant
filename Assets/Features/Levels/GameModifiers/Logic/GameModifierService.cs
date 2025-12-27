using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Features.Levels.GameModifiers.Data;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Effects.Logic;
using Features.Levels.GameModifiers.Events;
using Features.Levels.GameModifiers.Events.Data;
using UnityEngine;

namespace Features.Levels.GameModifiers.Logic
{
    public sealed class GameModifierService : IService
    {
        private EventModel _eventModel;

        private readonly Dictionary<(GameModifierData, EffectData), IEffectLogic> _logics = new();

        public void Initialize()
        {
            _eventModel = GameplayContext.Instance.Model.Events;
        }

        public void CleanUp() { }

        public void ApplyModifier(GameModifierData modifierData, Date endDate = null)
        {
            var origin = modifierData switch
            {
                EventGameModifierData eventData => new EffectOrigin("Event", eventData.Title),
                LevelGameModifierData conditionData => new EffectOrigin("Level Condition", conditionData.Title),
                _ => new EffectOrigin(modifierData.GetType().Name, "Unknown")
            };

            if (modifierData is EventGameModifierData eventModifierData)
            {
                if (endDate == null)
                {
                    Debug.LogError("Game events need an end date! Event was not executed.");
                    return;
                }

                _eventModel.OngoingEvents.Add(eventModifierData, endDate);
            }

            foreach (var effect in modifierData.Effects)
            {
                var logic = GetLogic(effect, origin);
                logic.Apply();
                _logics.Add((modifierData, effect), logic);
            }
        }

        public void RemoveModifier(GameModifierData modifierData)
        {
            if (modifierData is EventGameModifierData eventModifierData)
            {
                _eventModel.OngoingEvents.Remove(eventModifierData);
            }

            foreach (var effect in modifierData.Effects)
            {
                var key = (modifierData, effect);
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
                PriceEffectData effectData => new PriceEffectLogic(origin, effectData),
                MissionLimiterEffectData effectData => new MissionLimiterEffectLogic(origin, effectData),
                _ => throw new ArgumentOutOfRangeException(data.GetType().Name, data, null)
            };
        }
    }
}