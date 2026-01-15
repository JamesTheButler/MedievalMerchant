using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Utility;
using Features.Goods.Selector;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public sealed class MissionLimiterEffectLogic : EffectLogic<MissionLimiterEffectData>
    {
        public MissionLimiterEffectLogic(
            EffectOrigin effectOrigin,
            MissionLimiterEffectData effectData)
            : base(effectOrigin, effectData) { }

        public override void Apply()
        {
            var towns = GameplayContext.Instance.Model.Towns.Values;
            foreach (var town in towns)
            {
                if (!town.Regions.Intersects(EffectData.UnaffectedRegions))
                {
                    town.Missions.LimitGoodSelection(EffectData.GoodSelector.Selector);
                }
            }
        }

        public override void Unapply()
        {
            var towns = GameplayContext.Instance.Model.Towns.Values;
            foreach (var town in towns)
            {
                if (!town.Regions.Intersects(EffectData.UnaffectedRegions))
                {
                    town.Missions.LimitGoodSelection(IGoodSelector.All);
                }
            }
        }
    }
}