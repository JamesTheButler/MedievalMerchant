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
            foreach (var town in GameplayContext.Instance.Model.Towns.Values)
            {
                if (town.MainRegion == EffectData.AllyRegion)
                {
                    town.ReputationModel.UpdateReputation(
                        EffectData.StartReputationAlly,
                        "This town is your ally.");
                }
                else
                {
                    town.ReputationModel.UpdateReputation(EffectData.StartReputationOpponent,
                        "This town is your foe.");
                }
            }
        }

        public override void Unapply()
        {
            Debug.LogError($"{nameof(AllyEffectLogic)}.{nameof(Unapply)}() is not implemented.");
        }
    }
}