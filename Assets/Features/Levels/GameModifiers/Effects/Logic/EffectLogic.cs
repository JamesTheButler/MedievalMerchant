using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public abstract class EffectLogic<TEffectData> : IEffectLogic where TEffectData : EffectData
    {
        protected readonly EffectOrigin EffectOrigin;
        protected readonly TEffectData EffectData;

        public EffectLogic(EffectOrigin effectOrigin, TEffectData effectData)
        {
            EffectOrigin = effectOrigin;
            EffectData = effectData;
        }

        public abstract void Apply();
        public abstract void Unapply();
    }
}