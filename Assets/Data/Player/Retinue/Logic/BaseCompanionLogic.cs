using Data.Configuration;
using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public abstract class BaseCompanionLogic<T> : ICompanionLogic
        where T : CompanionConfigData
    {
        protected T ConfigData { get; }

        protected abstract CompanionType Type { get; }

        protected BaseCompanionLogic()
        {
            ConfigData = (T)ConfigurationManager.Instance.CompanionConfig.Get(Type);
        }

        public abstract void SetLevel(int level);
    }
}