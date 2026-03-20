using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public sealed class AllyEffectLogic : EffectLogic<AllyEffectData>
    {
        public AllyEffectLogic(EffectOrigin effectOrigin, AllyEffectData effectData) :
            base(effectOrigin, effectData) { }

        public override void Apply()
        {
            var loc = ResourceManager.Instance.LocalizationResources.Modifiers;
            foreach (var town in GameplayContext.Instance.Model.Towns.Values)
            {
                if (town.MainRegion == EffectData.AllyRegion)
                {
                    town.ReputationModel.UpdateReputation(EffectData.StartReputationAlly, loc.AllyEffect);
                }
                else
                {
                    town.ReputationModel.UpdateReputation(EffectData.StartReputationOpponent, loc.FoeEffect);
                }
            }
        }

        public override void Unapply()
        {
            Debug.LogError($"{nameof(AllyEffectLogic)}.{nameof(Unapply)}() is not implemented.");
        }
    }
}