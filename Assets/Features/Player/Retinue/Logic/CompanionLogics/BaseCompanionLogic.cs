using Common.Infrastructure;
using Features.Player.Retinue.Config;

namespace Features.Player.Retinue.Logic.CompanionLogics
{
    public abstract class BaseCompanionLogic<T> : ICompanionLogic
        where T : CompanionConfigData
    {
        protected T ConfigData { get; }

        protected abstract CompanionType Type { get; }

        protected BaseCompanionLogic()
        {
            ConfigData = (T)ConfigurationManager.Configurations.CompanionConfig.Get(Type);
        }

        public abstract void SetLevel(int level);
    }
}